namespace devdeer.DoctorFlox.Ui.WpfSample.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Commands;

    using Helpers;

    using Interfaces;

    using Models;

    /// <summary>
    /// The view model for the group-picker-window.
    /// </summary>
    public class PickGroupViewModel : BaseCollectionViewModel<GroupDataModel>
    {
        #region constructors and destructors

        /// <inheritdoc />
        public PickGroupViewModel()
        {
        }

        /// <inheritdoc />
        public PickGroupViewModel(IMessenger messenger) : base(messenger)
        {
        }

        /// <inheritdoc />
        public PickGroupViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
        }

        #endregion

        #region methods

        /// <inheritdoc />
        protected override void InitData()
        {
            base.InitData();
            InitItems(Variables.Groups);
        }

        /// <inheritdoc />
        protected override void InitCommands()
        {
            base.InitCommands();
            OkCommand = new RelayCommand(() => CloseWindow(true), () => CurrentItem != null);
            CancelCommand = new RelayCommand(CloseWindow);
        }

        /// <summary>
        /// Defines the logic for the OK-button.
        /// </summary>
        public RelayCommand OkCommand { get; private set; }

        /// <summary>
        /// Closes the associated window.
        /// </summary>
        public RelayCommand CancelCommand { get; private set; }

        #endregion
    }
}