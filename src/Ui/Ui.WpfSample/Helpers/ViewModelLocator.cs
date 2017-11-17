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
        public ChildViewModel ChildViewModel => Variables.AutoFacContainer.Resolve<ChildViewModel>();

        /// <summary>
        /// The view model for the list window.
        /// </summary>
        public CollectionViewModel CollectionViewModel => Variables.AutoFacContainer.Resolve<CollectionViewModel>();

        /// <summary>
        /// The view model for the main window.
        /// </summary>
        public MainViewModel MainViewModel => Variables.AutoFacContainer.Resolve<MainViewModel>();

        #endregion
    }
}