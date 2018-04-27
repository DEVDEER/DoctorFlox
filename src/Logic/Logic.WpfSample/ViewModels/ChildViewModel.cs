namespace devdeer.DoctorFlox.Logic.WpfSample.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    using Commands;

    using DoctorFlox.Enumerations;
    using DoctorFlox.Interfaces;

    using Interfaces;

    using Messages;

    using Models;

    /// <summary>
    /// The view model for the child window.
    /// </summary>
    public class ChildViewModel : BaseDataModelViewModel<ChildDataModel>
    {
        #region constructors and destructors

        /// <inheritdoc />
        public ChildViewModel()
        {
            TraceMethodName();
        }

        /// <inheritdoc />
        public ChildViewModel(ChildDataModel data) : base(data)
        {
            TraceMethodName();
        }

        /// <inheritdoc />
        public ChildViewModel(IMessenger messenger) : base(messenger)
        {
            TraceMethodName();
        }

        /// <inheritdoc />
        public ChildViewModel(IMessenger messenger, ChildDataModel data) : base(messenger, data)
        {
            TraceMethodName();
        }

        /// <inheritdoc />
        public ChildViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
            TraceMethodName();
        }

        /// <inheritdoc />
        public ChildViewModel(IMessenger messenger, SynchronizationContext synchronizationContext, ChildDataModel data) : base(messenger, synchronizationContext, data)
        {
            TraceMethodName();
        }

        #endregion

        #region methods

        /// <inheritdoc />
        public override void Cleanup()
        {
            base.Cleanup();
            TraceMethodName();
        }

        /// <inheritdoc />
        public override void OnInstanceActivated()
        {
            base.OnInstanceActivated();
            TraceMethodName();
        }

        /// <inheritdoc />
        public override void OnInstanceActivating()
        {
            base.OnInstanceActivating();
            TraceMethodName();
        }

        /// <inheritdoc />
        public override void OnWindowClosing()
        {
            base.OnWindowClosing();
            TraceMethodName();
        }

        /// <inheritdoc />
        protected override void AfterInitialization()
        {
            base.AfterInitialization();
            TraceMethodName();
        }

        /// <inheritdoc />
        protected override void InitCommands()
        {
            base.InitCommands();
            TraceMethodName();
            RebindCommand = new RelayCommand(
                () => UpdateData(
                    new ChildDataModel
                    {
                        Firstname = "First",
                        Lastname = "Last",
                        Age = 20
                    }));
            SelectGroupCommand = new RelayCommand(
                () =>
                {
                    Data.Group = GetResultFromCollectionViewModel<PickGroupViewModel, GroupDataModel, GroupDataModel>("PickGroupWindow", e => e.CurrentItem, Data.Group, Data.Group);
                });
            OkCommand = new RelayCommand(() => ShowMessageBox("OK"), () => IsOk);
            CancelCommand = new RelayCommand(CloseWindow);
        }

        /// <inheritdoc />
        protected override void InitData()
        {
            base.InitData();
            TraceMethodName();
        }

        /// <inheritdoc />
        protected override void InitMessenger()
        {
            base.InitMessenger();
            TraceMethodName();
            MessengerInstance.Register<DataMessage<MainViewModel, ChildViewModel, string>>(
                this,
                ThreadCallbackOption.UiThread,
                m =>
                {
                    MainViewMessage = m.Data;
                });
        }

        /// <inheritdoc />
        protected override void OnDataChanged(ChildDataModel oldValue)
        {
            base.OnDataChanged(oldValue);
            Trace.TraceInformation("Data changed.");
        }

        /// <summary>
        /// Writes a trace message to the output stream.
        /// </summary>
        /// <param name="method">The name of the calling method (is injected automatically).</param>
        private static void TraceMethodName([CallerMemberName] string method = null)
        {
            Trace.TraceInformation($"Called method [{method}].");
        }

        #endregion

        #region properties

        /// <summary>
        /// Closes the associated window.
        /// </summary>
        public RelayCommand CancelCommand { get; private set; }

        /// <inheritdoc />
        public override bool EnforceRaisePropertyChanged { get; set; } = true;

        /// <summary>
        /// Is populated by registering a certain message from the main view model.
        /// </summary>
        public string MainViewMessage { get; set; }

        /// <summary>
        /// Defines the logic for the OK-button.
        /// </summary>
        public RelayCommand OkCommand { get; private set; }

        /// <summary>
        /// Sets the Data property to a new value.
        /// </summary>
        public RelayCommand RebindCommand { get; private set; }

        public RelayCommand SelectGroupCommand { get; private set; }

        #endregion
    }
}