namespace devdeer.DoctorFlox.Logic.Wpf.Commands
{
    using System;
    using System.Linq;
    using System.Windows.Input;

    using Helpers;

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute
    /// method is 'true'.  This class does not allow you to accept command parameters in the
    /// Execute and CanExecute callback methods.
    /// </summary>
    public class RelayCommand : BaseRelayCommand
    {
        #region member vars

        private readonly WeakFunc<bool> _canExecute;
        private readonly WeakAction _execute;

        #endregion

        #region events

        /// <inheritdoc />
        public override event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Initializes a new instance of the RelayCommand class that
        /// can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <exception cref="ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <exception cref="ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            _execute = new WeakAction(execute);
            if (canExecute != null)
            {
                _canExecute = new WeakFunc<bool>(canExecute);
            }
        }

        #endregion

        #region methods

        /// <inheritdoc />
        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || (_canExecute.IsStatic || _canExecute.IsAlive) && _canExecute.Execute();
        }

        /// <inheritdoc />
        public override void Execute(object parameter)
        {
            if (CanExecute(parameter) && _execute != null && (_execute.IsStatic || _execute.IsAlive))
            {
                _execute.Execute();
            }
        }

        #endregion
    }

    /// <summary>
    /// A generic command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute
    /// method is 'true'. This class allows you to accept command parameters in the
    /// Execute and CanExecute callback methods.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    public class RelayCommand<T> : BaseRelayCommand
    {
        #region member vars

        private readonly WeakFunc<T, bool> _canExecute;
        private readonly WeakAction<T> _execute;

        #endregion

        #region events

        /// <inheritdoc />
        public override event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Initializes a new instance of the RelayCommand class that
        /// can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <exception cref="ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <exception cref="ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            _execute = new WeakAction<T>(execute);
            if (canExecute != null)
            {
                _canExecute = new WeakFunc<T, bool>(canExecute);
            }
        }

        #endregion

        #region methods

        /// <inheritdoc />
        public override bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            if (!_canExecute.IsStatic && !_canExecute.IsAlive)
            {
                return false;
            }
            if (parameter == null && typeof(T).IsValueType)
            {
                return _canExecute.Execute(default(T));
            }
            return _canExecute.Execute((T)parameter);
        }

        /// <inheritdoc />
        public override void Execute(object parameter)
        {
            var val = parameter;
            if (parameter != null && parameter.GetType() != typeof(T))
            {
                if (parameter is IConvertible)
                {
                    val = Convert.ChangeType(parameter, typeof(T), null);
                }
            }
            if (!CanExecute(val) || _execute == null || !_execute.IsStatic && !_execute.IsAlive)
            {
                return;
            }
            if (val == null)
            {
                if (typeof(T).IsValueType)
                {
                    _execute.Execute(default(T));
                }
                else
                {
                    _execute.Execute((T)val);
                }
            }
            else
            {
                _execute.Execute((T)val);
            }
        }

        #endregion
    }
}