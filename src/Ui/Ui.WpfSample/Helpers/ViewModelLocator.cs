namespace devdeer.DoctorFlox.Ui.WpfSample.Helpers
{
    using System;
    using System.Linq;

    using Autofac;

    using ViewModels;

    /// <summary>
    /// Provides instances of view models using AutoFac resolving.
    /// </summary>
    public class ViewModelLocator
    {
        #region properties

        /// <summary>
        /// The view model for the child window.
        /// </summary>
        public ChildViewModel ChildViewModel => BaseViewModel.IsInDesignModeStatic ? new ChildViewModel() : Variables.AutoFacContainer.Resolve<ChildViewModel>();

        /// <summary>
        /// The view model for the list window.
        /// </summary>
        public CollectionViewModel CollectionViewModel => BaseViewModel.IsInDesignModeStatic ? new CollectionViewModel() : Variables.AutoFacContainer.Resolve<CollectionViewModel>();

        /// <summary>
        /// The view model for the main window.
        /// </summary>
        public MainViewModel MainViewModel => BaseViewModel.IsInDesignModeStatic ? new MainViewModel() : Variables.AutoFacContainer.Resolve<MainViewModel>();

        /// <summary>
        /// The view model for the multi window.
        /// </summary>
        public MultiViewModel MultiViewModel => BaseViewModel.IsInDesignModeStatic ? new MultiViewModel() : Variables.AutoFacContainer.Resolve<MultiViewModel>();

        #endregion
    }
}