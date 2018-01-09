namespace devdeer.DoctorFlox.Ui.WpfSample.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Windows;

    
    using Commands;
    using Interfaces;

    /// <summary>
    /// The view model for the multi window which is used in multiple windows.
    /// </summary>
    public class MultiViewModel : BaseViewModel
    {
        #region constructors and destructors

        public MultiViewModel()
        {
        }

        public MultiViewModel(IMessenger messenger) : base(messenger)
        {
        }

        /// <inheritdoc />
        public MultiViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
        }

        #endregion

        #region methods

        /// <inheritdoc />
        protected override void InitCommands()
        {
            base.InitCommands();
            ShowMessageCommand = new RelayCommand(
                () =>
                {
                    MessageBox.Show($"I'm a message box with owner {AssociatedView.GetHashCode()}.");
                });
        }

        #endregion

        #region properties

        /// <summary>
        /// Shows a message box showing the <see cref="BaseViewModel.AssociatedView" /> hash code.
        /// </summary>
        public RelayCommand ShowMessageCommand { get; private set; }

        /// <summary>
        /// Returns the hash code of the <see cref="BaseViewModel.AssociatedView" />.
        /// </summary>
        public int WindowHashCode => AssociatedView.GetHashCode();

        #endregion
    }
}