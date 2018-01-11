namespace deveer.DoctorFlox.Tests.Logic.Core.TestModels
{
    using System;
    using System.Linq;
    using System.Threading;

    using devdeer.DoctorFlox;
    using devdeer.DoctorFlox.Commands;
    using devdeer.DoctorFlox.Interfaces;

    /// <summary>
    /// A type used in internal tests for <see cref="BaseViewModel" />.
    /// </summary>
    public class TestViewModel : BaseViewModel
    {
        #region constructors and destructors

        public TestViewModel()
        {
        }

        public TestViewModel(IMessenger messenger) : base(messenger)
        {
        }

        /// <inheritdoc />
        public TestViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
        }

        #endregion

        #region methods

        /// <inheritdoc />
        protected override void InitCommands()
        {
            base.InitCommands();
            TestCommand = new RelayCommand(
                () =>
                {
                });
        }

        /// <inheritdoc />
        protected override void InitData()
        {
            base.InitData();
            PropertySetByInitData = true;
        }

        #endregion

        #region properties

        /// <summary>
        /// A property that will only be set to <c>true</c> by <see cref="InitData" />.
        /// </summary>
        public bool PropertySetByInitData { get; private set; }

        /// <summary>
        /// A command for testing purposes only.
        /// </summary>
        public RelayCommand TestCommand { get; private set; }

        #endregion
    }
}