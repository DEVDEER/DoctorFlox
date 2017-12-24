namespace devdeer.DoctorFlox.Ui.WpfSample.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    using Logic.Wpf;
    using Logic.Wpf.Commands;
    using Logic.Wpf.Interfaces;

    using Models;

    /// <summary>
    /// The view model for the child window.
    /// </summary>
    public class ChildViewModel : BaseViewModel
    {
        #region constructors and destructors

        public ChildViewModel()
        {
            TraceMethodName();
            EnforceRaisePropertyChanged = true;
        }

        public ChildViewModel(IMessenger messenger) : base(messenger)
        {
            TraceMethodName();
            EnforceRaisePropertyChanged = true;
        }

        /// <inheritdoc />
        public ChildViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
            TraceMethodName();
            EnforceRaisePropertyChanged = true;
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
                () => Data = new ChildDataModel
                {
                    Firstname = "First",
                    Lastname = "Last",
                    Age = 20
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
        }

        /// <summary>
        /// Writes a trace message to the output stream.
        /// </summary>
        /// <param name="method">The name of the calling method (is injected automatically).</param>
        private void TraceMethodName([CallerMemberName] string method = null)
        {
            Trace.TraceInformation($"Called method [{method}].");
        }

        #endregion

        #region properties

        /// <summary>
        /// Closes the associated window.
        /// </summary>
        public RelayCommand CancelCommand { get; private set; }

        /// <summary>
        /// Represents the input data to bind to.
        /// </summary>
        public ChildDataModel Data { get; private set; } = new ChildDataModel();

        /// <summary>
        /// Defines the logic for the OK-button.
        /// </summary>
        public RelayCommand OkCommand { get; private set; }

        /// <summary>
        /// Sets the <see cref="Data" /> property to a new value.
        /// </summary>
        public RelayCommand RebindCommand { get; private set; }

        #endregion
    }
}