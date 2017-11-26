namespace devdeer.DoctorFlox.Logic.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Helpers;

    /// <summary>
    /// Abstract base class for all types which need to provide notification on property changes and
    /// support for <see cref="IDataErrorInfo" /> or the newer <see cref="INotifyDataErrorInfo" />.
    /// </summary>
    public abstract class BaseDataModel : BaseObservableObject, IDataErrorInfo, INotifyDataErrorInfo
    {
        #region member vars

        private readonly object _validationLock = new object();

        private IEnumerable<PropertyInfo> _propertiesToWatch;

        #endregion

        #region events

        /// <inheritdoc />
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion

        #region explicit interfaces

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string.
        /// </returns>
        [Browsable(false)]
        public string Error => HasErrors ? Errors.First().Value.First() : string.Empty;

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        /// <param name="columnName">The name of the property whose error message to get. </param>
        public string this[string columnName]
        {
            get
            {
                CollectErrors();
                return Errors.ContainsKey(columnName) ? Errors[columnName].First() : string.Empty;
            }
        }

        /// <inheritdoc />
        public IEnumerable GetErrors(string propertyName)
        {
            Errors.TryGetValue(propertyName, out var errors);
            return errors;
        }

        /// <summary>
        /// Indicates whether this instance has any errors.
        /// </summary>
        [Browsable(false)]
        public bool HasErrors => Errors.Any();

        #endregion

        #region methods

        /// <summary>
        /// Wrapper for securely invokation of <see cref="ErrorsChanged" />.
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnErrorsChanged(string propertyName)
        {
            // invoke the event
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            // Ensure that all dependend properties are updated
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(IsOk));
            OnPropertyChanged(nameof(Error));
        }

        /// <summary>
        /// Validates the complete instance.
        /// </summary>
        /// <remarks>
        /// Implemented thread safe.
        /// </remarks>
        public void Validate()
        {
            lock (_validationLock)
            {
                var validationContext = new ValidationContext(this, null, null);
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, validationContext, validationResults, true);
                foreach (var error in Errors.ToList())
                {
                    if (!validationResults.All(r => r.MemberNames.All(m => m != error.Key)))
                    {
                        continue;
                    }
                    if (Errors.TryRemove(error.Key, out var _))
                    {
                        OnErrorsChanged(error.Key);
                    }
                }
                HandleValidationResults(validationResults);
            }
        }

        /// <summary>
        /// Calls <see cref="Validate" /> on a thread-pool-thread.
        /// </summary>
        /// <returns>The awaitable task running the validation.</returns>
        public Task ValidateAsync()
        {
            return Task.Run(() => Validate());
        }

        /// <summary>
        /// Can be used to validate a single property with the given <paramref name="propertyName" />.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public void ValidateProperty([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }
            if (propertyName == "Item")
            {
                return;
            }
            lock (_validationLock)
            {
                var validationContext = new ValidationContext(this, null, null)
                {
                    MemberName = propertyName
                };
                var validationResults = new List<ValidationResult>();
                var value = GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)?.GetValue(this);
                Validator.TryValidateProperty(value, validationContext, validationResults);
                if (Errors.TryRemove(propertyName, out var _))
                {
                    OnErrorsChanged(propertyName);
                }
                HandleValidationResults(validationResults);
            }
        }

        /// <summary>
        /// Is called by the indexer to collect all errors and not only the one for a special field.
        /// </summary>
        /// <remarks>
        /// Because <see cref="HasErrors" /> depends on the <see cref="Errors" /> dictionary this
        /// ensures that controls like buttons can switch their state accordingly.
        /// </remarks>
        protected void CollectErrors()
        {
            Errors.Clear();
            var errors = ErrorValidationHelper.CollectErrors(this, CollapseInnerDataErrors);
            foreach (var error in errors)
            {
                Errors.TryAdd(error.Key, error.Value);
            }
            // we have to this because the Dictionary does not implement INotifyPropertyChanged            
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(IsOk));
            // commands do not recognize property changes automatically
            OnErrorsCollected();
        }

        /// <summary>
        /// Is called right before the <see cref="PropertiesToValidate" /> are defined once to give the child
        /// the oppurtunity to change the reflection-based list of properties to watch for validation.
        /// </summary>
        /// <param name="collectedProperties">The properties collected by reflection.</param>
        protected virtual void OnBeforeWatchedPropertiesDefined(List<PropertyInfo> collectedProperties)
        {
        }

        /// <summary>
        /// Can be overridden by derived types to react on the finisihing of error-collections.
        /// </summary>
        protected virtual void OnErrorsCollected()
        {
        }

        /// <inheritdoc />
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (PropertiesToValidate.Any(p => p.Name.Equals(propertyName)))
            {
                // this property should be validated
                ValidateProperty(propertyName);
            }
        }

        /// <summary>
        /// Central logic to handle validation results from <see cref="ValidationContext" />.
        /// </summary>
        /// <param name="validationResults">The collected validation results to handle.</param>
        private void HandleValidationResults(IEnumerable<ValidationResult> validationResults)
        {
            // get all results grouped by property names
            var resultsByPropNames = from validationResult in validationResults from memberName in validationResult.MemberNames group validationResult by memberName into g select g;
            foreach (var prop in resultsByPropNames)
            {
                var messages = prop.Select(r => r.ErrorMessage).ToList();
                // add each properties errors to the errors list
                if (Errors.TryAdd(prop.Key, messages))
                {
                    // if adding succeeded inform the UI
                    OnErrorsChanged(prop.Key);
                }
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// The opposite of <see cref="HasErrors" />.
        /// </summary>
        /// <remarks>
        /// Exists for convenient binding only.
        /// </remarks>
        public bool IsOk => !HasErrors;

        /// <summary>
        /// Indicates if inner errors of properties should be collapsed to the first found error.
        /// </summary>
        protected virtual bool CollapseInnerDataErrors => true;

        /// <summary>
        /// A dictionary of current errors with the name of the error-field as the key and the error
        /// text as the value.
        /// </summary>
        private ConcurrentDictionary<string, IEnumerable<string>> Errors { get; } = new ConcurrentDictionary<string, IEnumerable<string>>();

        /// <summary>
        /// Defines the properties this type should watch for validation when they are changed.
        /// </summary>
        private IEnumerable<PropertyInfo> PropertiesToValidate
        {
            get
            {
                if (_propertiesToWatch != null)
                {
                    return _propertiesToWatch;
                }
                var result = new List<PropertyInfo>();
                result.AddRange(ReflectionHelper.GetPropertiesWithDataErrorInfo(GetType()));
                result.AddRange(ReflectionHelper.GetPropertiesWithValidationAttribute(GetType()));
                OnBeforeWatchedPropertiesDefined(result);
                _propertiesToWatch = result.AsEnumerable();
                return _propertiesToWatch;
            }
        }

        #endregion
    }
}