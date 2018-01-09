namespace devdeer.DoctorFlox.Locators
{
    using System;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// This class defines the attached property and related change handler that calls the ViewModelLocator in Logic.Core.
    /// </summary>
    /// <remarks>
    /// Base logic taken from
    /// https://github.com/PrismLibrary/Prism/blob/a60d38013c02b60807e9287db9ba7f7506af0e84/Source/Windows10/Prism.Windows/Mvvm/ViewModelLocator.cs.
    /// </remarks>
    public static class AutoViewModelLocator
    {
        #region constants

        /// <summary>
        /// The AutoWireViewModel attached property.
        /// </summary>
        public static DependencyProperty AutoWireViewModelProperty = DependencyProperty.RegisterAttached(
            "AutoWireViewModel",
            typeof(bool),
            typeof(AutoViewModelLocator),
            new PropertyMetadata(false, AutoWireViewModelChanged));

        /// <summary>
        /// The ViewModelFqdn attached property.
        /// </summary>
        public static DependencyProperty ViewModelFqdnProperty = DependencyProperty.RegisterAttached(
            "ViewModelFqdn",
            typeof(string),
            typeof(AutoViewModelLocator),
            new PropertyMetadata(string.Empty, ViewModelFqdnChanged));

        #endregion

        #region methods

        /// <summary>
        /// Is used to obtain the value of the dependency property <see cref="AutoWireViewModelProperty" />.
        /// </summary>
        /// <param name="depObject">The element containing the attached property.</param>
        /// <returns>The current value of <see cref="AutoWireViewModelProperty" />.</returns>
        public static bool GetAutoWireViewModel(DependencyObject depObject)
        {
            if (depObject == null)
            {
                throw new ArgumentNullException(nameof(depObject));
            }
            return (bool)depObject.GetValue(AutoWireViewModelProperty);
        }

        /// <summary>
        /// Is used to obtain the value of the dependency property <see cref="ViewModelFqdnProperty" />.
        /// </summary>
        /// <param name="depObject">The element containing the attached property.</param>
        /// <returns>The current value of <see cref="ViewModelFqdnProperty" />.</returns>
        public static string GetViewModelFqdn(DependencyObject depObject)
        {
            if (depObject == null)
            {
                throw new ArgumentNullException(nameof(depObject));
            }
            return (string)depObject.GetValue(ViewModelFqdnProperty);
        }

        /// <summary>
        /// Is used to change the value of the dependency property <see cref="AutoWireViewModelProperty" />.
        /// </summary>
        /// <param name="depObject">The element containing the attached property.</param>
        /// <param name="value">The new value for the <see cref="AutoWireViewModelProperty" />.</param>
        public static void SetAutoWireViewModel(DependencyObject depObject, bool value)
        {
            depObject.SetValue(AutoWireViewModelProperty, value);
        }

        /// <summary>
        /// Is used to change the value of the dependency property <see cref="ViewModelFqdnProperty" />.
        /// </summary>
        /// <param name="depObject">The element containing the attached property.</param>
        /// <param name="value">The new value for the <see cref="ViewModelFqdnProperty" />.</param>
        public static void SetViewModelFqdn(DependencyObject depObject, string value)
        {
            depObject.SetValue(ViewModelFqdnProperty, value);
        }

        /// <summary>
        /// Is called whenever <see cref="AutoWireViewModelProperty" /> changes it's value.
        /// </summary>
        /// <param name="depObject">The element containing the attached property.</param>
        /// <param name="e">The event arguments.</param>
        private static void AutoWireViewModelChanged(DependencyObject depObject, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ViewModelLocationProvider.AutoWireViewModelChanged(depObject, Bind);
            }
        }

        /// <summary>
        /// Sets the DataContext of a View
        /// </summary>
        /// <param name="view">The View to set the DataContext on</param>
        /// <param name="viewModel">The object to use as the DataContext for the View</param>
        private static void Bind(object view, object viewModel)
        {
            if (view is FrameworkElement element)
            {
                element.DataContext = viewModel;
            }
        }

        /// <summary>
        /// Is called whenever <see cref="ViewModelFqdnProperty" /> changes it's value.
        /// </summary>
        /// <param name="depObject">The element containing the attached property.</param>
        /// <param name="e">The event arguments.</param>
        private static void ViewModelFqdnChanged(DependencyObject depObject, DependencyPropertyChangedEventArgs e)
        {
            ViewModelLocationProvider.ViewModelFqdnChanged(depObject, e.NewValue.ToString(), Bind);
        }

        #endregion
    }
}