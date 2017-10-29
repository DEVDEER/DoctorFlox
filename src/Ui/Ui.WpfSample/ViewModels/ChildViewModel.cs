namespace devdeer.DoctorFlox.Ui.WpfSample.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    using Logic.Wpf;
    using Logic.Wpf.Interfaces;

    /// <summary>
    /// The view model for the child window.
    /// </summary>
    public class ChildViewModel : BaseViewModel
    {
        #region constructors and destructors

        /// <inheritdoc />
        public ChildViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
            // some comment
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
    }
}