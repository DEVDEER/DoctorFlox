namespace devdeer.DoctorFlox.Logic.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Windows;

    using Attributes;

    using Commands;

    using Helpers;

    using Interfaces;

    using Messages;

    /// <summary>
    /// Base class for a view model.
    /// </summary>
    public abstract class BaseViewModel : BaseDataModel, ICleanup
    {
        #region member vars

        private readonly IMessenger _messenger;

        private IEnumerable<BaseRelayCommand> _commands;

        #endregion

        #region constants

        /// <summary>
        /// Defines the singleton logic to determine if this instance is running in designers or at runtime.
        /// </summary>
        private static readonly Lazy<bool> IsInDesignModeFactory = new Lazy<bool>(
            () =>
            {
                var designModeProperty = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(designModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
            });

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseViewModel() : this(Messenger.Default)
        {
        }

        /// <summary>
        /// Constructor for passing <paramref name="messenger" /> from outside.
        /// </summary>
        /// <param name="messenger">The messenger to use.</param>
        public BaseViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            PerformConstructorCalls();
        }

        /// <summary>
        /// Constructor for passing <paramref name="messenger" /> and <paramref name="synchronizationContext" /> from outside.
        /// </summary>
        /// <param name="messenger">The messenger to use.</param>
        /// <param name="synchronizationContext">The sync context to use.</param>
        public BaseViewModel(IMessenger messenger, SynchronizationContext synchronizationContext)
        {
            _messenger = messenger;
            PerformConstructorCalls();
            SyncContext = synchronizationContext;
        }

        #endregion

        #region explicit interfaces

        /// <summary>
        /// Unregisters this instance from the Messenger class.
        /// <para>
        /// To cleanup additional resources, override this method, clean
        /// up and then call base.Cleanup().
        /// </para>
        /// </summary>
        public virtual void Cleanup()
        {
            MessengerInstance.Unregister(this);
        }

        #endregion

        #region methods

        /// <summary>
        /// Should be called from outside to signal that a new instance was attached.
        /// </summary>
        /// <remarks>
        /// Calls <see cref="AfterInitialization" /> internally. Be sure to recall this base
        /// implementation in overridings.
        /// </remarks>
        public virtual void OnInstanceActivated()
        {
            AfterInitialization();
        }

        /// <summary>
        /// Should be called from outside to signal that a new instance is about to be attached.
        /// </summary>
        public virtual void OnInstanceActivating()
        {
        }

        /// <summary>
        /// Is called by the <see cref="WindowClosingCommand" />.
        /// </summary>
        /// <remarks>
        /// Calls <see cref="Cleanup" /> internally.
        /// </remarks>
        public virtual void OnWindowClosing()
        {
            Cleanup();
        }

        /// <summary>
        /// Opens a message box with the given options and the <see cref="AssociatedView" /> as the owner and retrieves the result.
        /// </summary>
        /// <param name="messageText">The text of the message.</param>
        /// <param name="caption">The optional caption (defaults to <c>null</c>).</param>
        /// <param name="button">The buttons to display (defaults to <see cref="MessageBoxButton.OK" />).</param>
        /// <param name="image">The icon to show (defaults to <see cref="MessageBoxImage.None" />).</param>
        /// <param name="defaultResult">The default result to take (defaults to <see cref="MessageBoxResult.None" />).</param>
        /// <returns>The result the user choose.</returns>
        public virtual MessageBoxResult ShowMessageBox(
            string messageText,
            string caption = null,
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.None,
            MessageBoxResult defaultResult = MessageBoxResult.None)
        {
            return MessageBox.Show(AssociatedView, messageText, caption, button, image, defaultResult);
        }

        /// <summary>
        /// Can be overridden to perform logic that should be called after all init-procedures.
        /// </summary>
        /// <remarks>
        /// Do not implement long-term-operations here because this method is called by the ctor.
        /// </remarks>
        protected virtual void AfterInitialization()
        {
        }

        /// <summary>
        /// Can be used to close the window which holds the data context to this type.
        /// </summary>
        protected void CloseWindow()
        {
            AssociatedView?.Close();
        }

        /// <summary>
        /// Uses <see cref="ReflectionHelper.ViewTypeListFactory" /> to search for a given <paramref name="viewType" /> and
        /// retrieve an instance of
        /// it.
        /// </summary>
        /// <param name="viewType">The type of the window the caller wants to open.</param>
        /// <param name="throwException"><c>true</c> if this method should throw exceptions (defaults to <c>true</c>).</param>
        /// <param name="owner">The handle for the owner window.</param>
        /// <returns>The window instance or <c>null</c> if an error occurs.</returns>
        protected Window CreateWindowInstance(Type viewType, bool throwException = true, Window owner = null)
        {
            if (viewType == null)
            {
                if (throwException)
                {
                    throw new ArgumentNullException(nameof(viewType));
                }
                return null;
            }
            try
            {
                return WindowInstanceResolver.Invoke(viewType);
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw new InvalidOperationException("Could not create instance of desired window type.", ex);
                }
            }
            return null;
        }

        /// <summary>
        /// Uses <see cref="ReflectionHelper.ViewTypeListFactory" /> to search for a type with the given
        /// <paramref name="viewTypeName" /> and
        /// retrieve an instance of it.
        /// </summary>
        /// <param name="viewTypeName">The name or full name of the type we are searching for.</param>
        /// <param name="throwException"><c>true</c> if this method should throw exceptions (defaults to <c>true</c>).</param>
        /// <param name="owner">The handle for the owner window.</param>
        /// <returns>The window instance or <c>null</c> if an error occurs.</returns>
        protected Window CreateWindowInstance(string viewTypeName, bool throwException = true, Window owner = null)
        {
            var viewType = ReflectionHelper.GetViewTypeByNameOrFullName(viewTypeName);
            return viewType == null ? null : CreateWindowInstance(viewType, throwException, owner);
        }

        /// <summary>
        /// Searches for a type in all currently opened windows.
        /// </summary>
        /// <param name="windowType">The type of the window to search for.</param>
        /// <returns>The window instance or <c>null</c> if no result was found.</returns>
        protected Window GetWindowInstance(Type windowType)
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window.GetType() == windowType)
                {
                    return window as Window;
                }
            }
            return null;
        }

        /// <summary>
        /// Searches for a type in all currently opened windows.
        /// </summary>
        /// <param name="windowTypeName">
        /// Optional name type of the window to search for. If set to <c>null</c>, the
        /// <see cref="AssociatedView" /> window will be resolved.
        /// </param>
        /// <returns>The window instance or <c>null</c> if no result was found.</returns>
        protected Window GetWindowInstance(string windowTypeName = null)
        {
            if (windowTypeName == null)
            {
                // try to return the associated view
                return AssociatedView;
            }
            foreach (var window in Application.Current.Windows)
            {
                var type = window.GetType();
                if (type.Name.Equals(windowTypeName, StringComparison.Ordinal) || (type.FullName ?? string.Empty).Equals(windowTypeName, StringComparison.Ordinal))
                {
                    return window as Window;
                }
            }
            return null;
        }

        /// <summary>
        /// Can be overridden to define the commands.
        /// </summary>
        /// <remarks>
        /// Do not implement long-term-operations here because this method is called by the ctor.
        /// </remarks>
        protected virtual void InitCommands()
        {
        }

        /// <summary>
        /// Can be overridden to start any data load operation during runtime.
        /// </summary>
        /// <remarks>
        /// Do not implement long-term-operations here because this method is called by the ctor.
        /// </remarks>
        protected virtual void InitData()
        {
        }

        /// <summary>
        /// Can be overridden to define the state of this view model during design time.
        /// </summary>
        /// <remarks>
        /// Do not implement long-term-operations here because this method is called by the ctor.
        /// </remarks>
        protected virtual void InitDesignTimeData()
        {
        }

        /// <summary>
        /// Can be overridden to hook up into the messenger pipeline.
        /// </summary>
        /// <remarks>
        /// Do not implement long-term-operations here because this method is called by the ctor.
        /// </remarks>
        protected virtual void InitMessenger()
        {
        }

        /// <inheritdoc />
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (EnforceRaisePropertyChanged)
            {
                // We want to check all command execution states again.
                Commands.ToList().ForEach(c => c.RaiseCanExecuteChanged());
            }
        }

        /// <summary>
        /// Private implementation of command initialization.
        /// </summary>
        /// <remarks>
        /// This calls <see cref="InitCommands" /> after the internal stuff is completed so that the caller can not
        /// forget to call base there.
        /// </remarks>
        private void InternalInitCommands()
        {
            WindowClosingCommand = new RelayCommand(OnWindowClosing);
            InitCommands();
        }

        /// <summary>
        /// Is called by all constructors to call virtual methods useful for all children.
        /// </summary>
        private void PerformConstructorCalls()
        {
            if (IsInDesignMode)
            {
                InitDesignTimeData();
            }
            else
            {
                InternalInitCommands();
                InitMessenger();
                InitData();
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// Tries to retrieve the window associated with this view.
        /// </summary>
        /// <remarks>
        /// This logic tries to find any <see cref="ViewTypeNameAttribute" /> which this type is decorated with. If none is found
        /// this logic
        /// assumes that the naming conventions says that view models are using the string 'ViewModel' in their name where views
        /// will use 'Window'.
        /// </remarks>
        public Window AssociatedView
        {
            get
            {
                var attribute = ViewTypeNameAttribute;
                if (attribute != null)
                {
                    // we have to retrieve the type
                    return CreateWindowInstance(attribute.ViewType, false);
                }
                // try to retrieve type using name conventions
                var type = ReflectionHelper.GetViewTypeByNameOrFullName(GetType().Name.Replace("ViewModel", "Window"));
                return type == null ? null : GetWindowInstance(type);
            }
        }

        /// <summary>
        /// Indicates if <see cref="BaseRelayCommand.RaiseCanExecuteChanged" /> should be called for each command
        /// after one of the properties of this instances has changed.
        /// </summary>
        public bool EnforceRaisePropertyChanged { get; set; } = false;

        /// <summary>
        /// Indicates if this instance is opened by a designer currrently.
        /// </summary>
        /// <remarks>
        /// Calls <see cref="IsInDesignModeStatic" /> internally.
        /// </remarks>
        public bool IsInDesignMode => IsInDesignModeStatic;

        /// <summary>
        /// Indicates if this instance is opened by a designer currrently.
        /// </summary>
        public static bool IsInDesignModeStatic => IsInDesignModeFactory.Value;

        /// <summary>
        /// Retrieves the <see cref="ViewTypeNameAttribute" /> associated to this type if there is any, otherwise <c>null</c> is
        /// returned.
        /// </summary>
        public AssociatedViewAttribute ViewTypeNameAttribute => ReflectionHelper.GetViewTypeNameAttribute(GetType());

        /// <summary>
        /// Should be bound against the Closing event of the associated window using <see cref="EventToCommand" />.
        /// </summary>
        /// <remarks>
        /// Will call <see cref="OnWindowClosing" />.
        /// </remarks>
        public RelayCommand WindowClosingCommand { get; private set; }

        /// <summary>
        /// A messenger passed in by DI or the default one.
        /// </summary>
        protected IMessenger MessengerInstance => _messenger ?? Messenger.Default;

        /// <summary>
        /// The synchronization context to use internally.
        /// </summary>
        protected SynchronizationContext SyncContext { get; set; }

        /// <summary>
        /// Defines how this view model will resolve a given type into an instance of type <see cref="Window" />.
        /// </summary>
        protected virtual Func<Type, Window> WindowInstanceResolver { get; } = type => Activator.CreateInstance(type) as Window;

        /// <summary>
        /// Retrieves the list of commands provided in this instance.
        /// </summary>
        /// <remarks>
        /// Is implemented as a lazy property.
        /// </remarks>
        private IEnumerable<BaseRelayCommand> Commands
        {
            get
            {
                return _commands ?? (_commands = GetType().GetProperties().Where(p => typeof(BaseRelayCommand).IsAssignableFrom(p.PropertyType)).Select(c => c.GetValue(this))
                           .Cast<BaseRelayCommand>());
            }
        }

        #endregion
    }
}