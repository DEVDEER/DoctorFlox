namespace devdeer.DoctorFlox
{
    using System;
    using System.Linq;
    using System.Threading;

    using EventArguments;

    using Interfaces;

    using Messages;

    /// <summary>
    /// Abstract base class for view models which will hold an instance of type
    /// <typeparamref name="TDataModel" />.
    /// </summary>
    /// <typeparam name="TDataModel">The type of the <see cref="Data" /> representing data shown in the view.</typeparam>
    /// <remarks>
    /// You can consume the <see cref="DataChanged" /> event to react on replacements of the <see cref="Data" /> property. This
    /// won't be called if the instance does not change but the data in it. Use <see cref="PropertyChanged" /> in this case.
    /// </remarks>
    public abstract class BaseDataModelViewModel<TDataModel> : BaseViewModel
        where TDataModel : BaseDataModel, new()
    {
        #region member vars

        private TDataModel _data;

        #endregion

        #region events

        /// <summary>
        /// Occurs after the value of <see cref="Data" /> has changed.
        /// </summary>
        public event EventHandler<DataModelChangedEventArgs<TDataModel>> DataChanged;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseDataModelViewModel() : base(Messenger.Default)
        {
        }

        /// <summary>
        /// Constructor allowing passing of <paramref name="data" /> from outside.
        /// </summary>
        public BaseDataModelViewModel(TDataModel data) : base(Messenger.Default)
        {
            Data = data;
        }

        /// <summary>
        /// Constructor for passing <paramref name="messenger" /> from outside.
        /// </summary>
        /// <param name="messenger">The messenger to use.</param>
        public BaseDataModelViewModel(IMessenger messenger) : base(messenger)
        {
        }

        /// <summary>
        /// Constructor for passing <paramref name="messenger" /> and <paramref name="data" /> from outside.
        /// </summary>
        /// <param name="messenger">The messenger to use.</param>
        /// <param name="data">The data the view will bind controls against.</param>
        public BaseDataModelViewModel(IMessenger messenger, TDataModel data) : base(messenger)
        {
            Data = data;
        }

        /// <summary>
        /// Constructor for passing <paramref name="messenger" /> and <paramref name="synchronizationContext" /> from outside.
        /// </summary>
        /// <param name="messenger">The messenger to use.</param>
        /// <param name="synchronizationContext">The sync context to use.</param>
        public BaseDataModelViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
        }

        /// <summary>
        /// Constructor for passing <paramref name="messenger" />, <paramref name="synchronizationContext" /> and
        /// <paramref name="data" /> from outside.
        /// </summary>
        /// <param name="messenger">The messenger to use.</param>
        /// <param name="synchronizationContext">The sync context to use.</param>
        /// <param name="data">The data the view will bind controls against.</param>
        public BaseDataModelViewModel(IMessenger messenger, SynchronizationContext synchronizationContext, TDataModel data) : base(messenger, synchronizationContext)
        {
            Data = data;
        }

        #endregion

        #region methods

        /// <summary>
        /// Can be called to change <see cref="Data" /> from outside.
        /// </summary>
        /// <param name="data">The data the view will bind controls against.</param>
        public void UpdateData(TDataModel data)
        {
            if (Data == data)
            {
                return;
            }
            Data = data;
        }

        /// <summary>
        /// Is called by the setter of <see cref="Data" /> when the property has got a new value.
        /// </summary>
        /// <remarks>
        /// Be sure to re-call this base implementation to raise the <see cref="DataChanged" /> event.
        /// </remarks>
        /// <param name="oldValue">The value of <see cref="Data" /> before the change happened.</param>
        protected virtual void OnDataChanged(TDataModel oldValue)
        {
            DataChanged?.Invoke(this, new DataModelChangedEventArgs<TDataModel>(oldValue, _data));
        }

        #endregion

        #region properties

        /// <summary>
        /// The data the view will bind controls against.
        /// </summary>
        /// <remarks>
        /// Raises <see cref="DataChanged" /> event whenever it's value changes.
        /// </remarks>
        public TDataModel Data
        {
            get => _data ?? (_data = new TDataModel());
            private set
            {
                if (Data == value)
                {
                    return;
                }
                var oldValue = _data;
                _data = value;
                OnPropertyChanged();
                OnDataChanged(oldValue);
            }
        }

        #endregion
    }
}