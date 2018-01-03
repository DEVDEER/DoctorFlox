namespace devdeer.DoctorFlox.Ui.AutoWireupSample.ViewModels
{
    using System;
    using System.Linq;

    using Logic.Wpf;
    using Logic.Wpf.Locators;

    using Views;

    /// <summary>
    /// The view model for the <see cref="MainView" />.
    /// </summary>
    /// <remarks>
    /// This should be automatically resolved as the DataContext for the main view because
    /// of the naming we use here. The <see cref="AutoViewModelLocator" /> is bound in the MainView.xaml
    /// and will search for a MainViewModel if the window is called MainView. The parts 'ViewModel' and 'View'
    /// in those names are key.
    /// </remarks>
    public class MainViewModel : BaseViewModel
    {
        #region properties

        /// <summary>
        /// The caption for the view.
        /// </summary>
        public string Caption { get; set; } = "Autowire";

        #endregion
    }
}