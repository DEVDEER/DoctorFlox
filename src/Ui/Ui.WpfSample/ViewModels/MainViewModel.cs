namespace devdeer.DoctorFlox.Ui.WpfSample.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    using Autofac;

    using Commands;

    using DoctorFlox.Enumerations;

    using Enumerations;

    using Helpers;

    using Interfaces;

    using Messages;

    using Models.Messages;

    /// <summary>
    /// View model for the main window.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region constructors and destructors

        public MainViewModel()
        {
        }

        public MainViewModel(IMessenger messenger) : base(messenger)
        {
        }

        public MainViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
        }

        #endregion

        #region methods

        /// <inheritdoc />
        protected override void AfterInitialization()
        {
            base.AfterInitialization();
            Task.Delay(2000).ContinueWith(
                t =>
                {
                    Trace.TraceInformation($"Sending message from thread {Thread.CurrentThread.ManagedThreadId}");
                    MessengerInstance.Send(new DataMessage<MainViewModel, MainViewModel, string>("Hello"));
                });
        }

        /// <inheritdoc />
        protected override void InitCommands()
        {
            base.InitCommands();
            ShowMessageCommand = new RelayCommand(
                () =>
                {
                    ShowMessageBox($"You said: {TestMessage}", "Test Message");
                },
                () => !string.IsNullOrEmpty(TestMessage));
            OpenChildWindowCommand = new RelayCommand(
                () =>
                {
                    if (UseMessengerForWindowActions)
                    {
                        MessengerInstance.Send(new WindowOpenRequestMessage(WindowType.ChildWindow));
                        return;
                    }
                    var windowInstance = CreateWindowInstance("ChildWindow");
                    MessengerInstance.Send(new DataMessage<MainViewModel, ChildViewModel, string>(this, "Hello World!"));
                    windowInstance?.ShowDialog();
                });
            OpenCollectionWindowCommand = new RelayCommand(
                () =>
                {
                    if (UseMessengerForWindowActions)
                    {
                        MessengerInstance.Send(new WindowOpenRequestMessage(WindowType.CollectionWindow));
                        return;
                    }
                    var windowInstance = CreateWindowInstance("CollectionWindow");
                    windowInstance?.ShowDialog();
                });
            OpenMultiWindowCommand = new RelayCommand(
                () =>
                {
                    for (var i = 0; i < 3; i++)
                    {
                        var windowInstance = CreateWindowInstance("MultiWindow");
                        windowInstance?.Show();
                    }
                });
            TestAsyncMessengerCommand = new RelayCommand(RunAsyncMessengerSample, () => !IsProgressOperationRunning);
        }

        /// <inheritdoc />
        protected override void InitData()
        {
            base.InitData();
            Caption = "DoctorFlox WpfSample (Runtime)";
        }

        /// <inheritdoc />
        protected override void InitDesignTimeData()
        {
            base.InitDesignTimeData();
            Caption = "DoctorFlox WpfSample (Design)";
        }

        /// <inheritdoc />
        protected override void InitMessenger()
        {
            base.InitMessenger();
            MessengerInstance.Register<DataMessage<MainViewModel, MainViewModel, string>>(
                this,
                ThreadCallbackOption.UiThread,
                m =>
                {
                    Trace.TraceInformation($"Receiving message on thread {Thread.CurrentThread.ManagedThreadId}");
                    Message = m.Data;
                });
            MessengerInstance.Register<DataMessage<MainViewModel, MainViewModel, int>>(
                this,
                ThreadCallbackOption.UiThread,
                async m =>
                {
                    Progress = m.Data;
                    IsProgressOperationRunning = true;
                    if (Progress == 100)
                    {
                        await Task.Delay(200);
                        IsProgressOperationRunning = false;
                        Progress = 0;
                    }
                });
        }

        /// <summary>
        /// Starts a task which will post a <see cref="DataMessage{TSender,TTarget,TData}" /> on a new threadpool-thread.
        /// </summary>
        /// <remarks>
        /// See the <see cref="InitMessenger" /> method for the message registration.
        /// </remarks>
        private void RunAsyncMessengerSample()
        {
            Progress = 0;
            Task.Run(
                async () =>
                {
                    for (var progress = 0; progress <= 100; progress += 10)
                    {
                        MessengerInstance.Send(new DataMessage<MainViewModel, MainViewModel, int>(progress));
                        await Task.Delay(200);
                    }
                });
        }

        #endregion

        #region properties

        /// <summary>
        /// The caption for the view.
        /// </summary>
        public string Caption { get; private set; }

        /// <summary>
        /// Indicates if there is currently an operation which will change the value of <see cref="Progress" />.
        /// </summary>
        public bool IsProgressOperationRunning { get; private set; }

        /// <summary>
        /// The last message arrived.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Triggers the opening of a new child window.
        /// </summary>
        public RelayCommand OpenChildWindowCommand { get; private set; }

        /// <summary>
        /// Triggers the opening of a new collection window.
        /// </summary>
        public RelayCommand OpenCollectionWindowCommand { get; private set; }

        /// <summary>
        /// Triggers the opening 3 different multi-windows at once.
        /// </summary>
        public RelayCommand OpenMultiWindowCommand { get; private set; }

        /// <summary>
        /// A progress which can take values from 0 to 100.
        /// </summary>
        public int Progress { get; private set; }

        /// <summary>
        /// Can be used to show a message box.
        /// </summary>
        public RelayCommand ShowMessageCommand { get; private set; }

        /// <summary>
        /// Triggers an async method which will perform some task and then sends a message
        /// back to trigger some UI changes.
        /// </summary>
        public RelayCommand TestAsyncMessengerCommand { get; private set; }

        /// <summary>
        /// Some test text from the UI.
        /// </summary>
        public string TestMessage { get; set; }

        /// <summary>
        /// If set to <c>true</c> all window-related events will be propagated via the <see cref="Messenger" />
        /// instead of using reflection based
        /// <see cref="BaseViewModel.CreateWindowInstance(System.Type,bool,System.Windows.Window)" />.
        /// </summary>
        public bool UseMessengerForWindowActions { get; set; }

        /// <inheritdoc />
        protected override Func<Type, Window> WindowInstanceResolver
        {
            get
            {
                return viewType => Variables.AutoFacContainer.Resolve(viewType) as Window;
            }
        }

        #endregion
    }
}