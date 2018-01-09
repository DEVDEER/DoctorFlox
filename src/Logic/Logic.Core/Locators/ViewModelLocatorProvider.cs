namespace devdeer.DoctorFlox.Locators
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The ViewModelLocationProvider class locates the view model for the view that has the AutoWireViewModelChanged attached
    /// property set to true.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The view model will be located and injected into the view's DataContext. To locate the view, two strategies are used:
    /// First the ViewModelLocationProvider will look to see if there is a view model factory registered for that view, if not
    /// it will try to infer the view model using a convention based approach.
    /// </para>
    /// <para>
    /// This class also provide methods for registering the view model factories, and also to override the default view model
    /// factory and the default view type to view model type resolver.
    /// </para>
    /// <para>
    /// Base implementation from
    /// https://github.com/PrismLibrary/Prism/blob/master/Source/Prism/Mvvm/ViewModelLocationProvider.cs
    /// </para>
    /// </remarks>
    public static class ViewModelLocationProvider
    {
        #region constants

        /// <summary>
        /// The default view model factory which provides the ViewModel type as a parameter.
        /// </summary>
        private static Func<Type, object> _defaultViewModelFactory = Activator.CreateInstance;

        /// <summary>
        /// ViewModelfactory that provides the View instance and ViewModel type as parameters.
        /// </summary>
        private static Func<object, Type, BaseViewModel> _defaultViewModelFactoryWithViewParameter;

        /// <summary>
        /// Default view type to view model type resolver, assumes the view model is in same assembly as the view type, but in the
        /// "ViewModels" namespace.
        /// </summary>
        private static Func<Type, Type> _defaultViewTypeToViewModelTypeResolver = viewType =>
        {
            var viewName = viewType.FullName;
            if (string.IsNullOrEmpty(viewName))
            {
                return null;
            }
            viewName = viewName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewName, suffix, viewAssemblyName);
            return Type.GetType(viewModelName);
        };

        /// <summary>
        /// A dictionary that contains all the registered factories for the views.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Func<object>> Factories = new ConcurrentDictionary<string, Func<object>>();

        /// <summary>
        /// View type to view model type resolver, assumes the view model is in same assembly as the view type and under the given
        /// FQDN.
        /// </summary>
        private static readonly Func<Type, string, Type> FqdnViewTypeToViewModelTypeResolver = (viewType, viewModelFqdn) =>
        {
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = $"{viewModelFqdn}, {viewAssemblyName}";
            return Type.GetType(viewModelName);
        };

        /// <summary>
        /// A dictionary that contains all the registered ViewModel types for the views.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Type> TypeFactories = new ConcurrentDictionary<string, Type>();

        #endregion

        #region methods

        /// <summary>
        /// Automatically looks up the viewmodel that corresponds to the current view, using two strategies:
        /// It first looks to see if there is a mapping registered for that view, if not it will fallback to the convention based
        /// approach.
        /// </summary>
        /// <param name="view">The dependency object, typically a view.</param>
        /// <param name="setDataContextCallback">The call back to use to create the binding between the View and ViewModel</param>
        public static void AutoWireViewModelChanged(object view, Action<object, object> setDataContextCallback)
        {
            // Try mappings first
            var viewModel = GetViewModelForView(view);
            // try to use ViewModel type
            if (viewModel == null)
            {
                //check type mappings
                var viewModelType = GetViewModelTypeForView(view.GetType()) ?? _defaultViewTypeToViewModelTypeResolver(view.GetType());
                // fallback to convention based
                if (viewModelType == null)
                {
                    return;
                }
                viewModel = _defaultViewModelFactoryWithViewParameter != null ? _defaultViewModelFactoryWithViewParameter(view, viewModelType) : _defaultViewModelFactory(viewModelType);
            }
            setDataContextCallback(view, viewModel);
        }

        /// <summary>
        /// Registers the ViewModel factory for the specified view type.
        /// </summary>
        /// <typeparam name="TView">The View</typeparam>
        /// <param name="factory">The ViewModel factory.</param>
        public static void Register<TView>(Func<object> factory)
        {
            Register(typeof(TView).ToString(), factory);
        }

        /// <summary>
        /// Registers the ViewModel factory for the specified view type name.
        /// </summary>
        /// <param name="viewTypeName">The name of the view type.</param>
        /// <param name="factory">The ViewModel factory.</param>
        public static void Register(string viewTypeName, Func<object> factory)
        {
            Factories[viewTypeName] = factory;
        }

        /// <summary>
        /// Registers a ViewModel type for the specified view type.
        /// </summary>
        /// <typeparam name="TView">The View</typeparam>
        /// <typeparam name="TViewModel">The ViewModel</typeparam>
        public static void Register<TView, TViewModel>()
        {
            var viewType = typeof(TView);
            var viewModelType = typeof(TViewModel);
            Register(viewType.ToString(), viewModelType);
        }

        /// <summary>
        /// Registers a ViewModel type for the specified view.
        /// </summary>
        /// <param name="viewTypeName">The View type name</param>
        /// <param name="viewModelType">The ViewModel type</param>
        public static void Register(string viewTypeName, Type viewModelType)
        {
            TypeFactories[viewTypeName] = viewModelType;
        }

        /// <summary>
        /// Sets the default view model factory.
        /// </summary>
        /// <param name="viewModelFactory">The view model factory which provides the ViewModel type as a parameter.</param>
        public static void SetDefaultViewModelFactory(Func<Type, object> viewModelFactory)
        {
            _defaultViewModelFactory = viewModelFactory;
        }

        /// <summary>
        /// Sets the default view model factory.
        /// </summary>
        /// <param name="viewModelFactory">The view model factory that provides the View instance and ViewModel type as parameters.</param>
        public static void SetDefaultViewModelFactory(Func<object, Type, BaseViewModel> viewModelFactory)
        {
            _defaultViewModelFactoryWithViewParameter = viewModelFactory;
        }

        /// <summary>
        /// Sets the default view type to view model type resolver.
        /// </summary>
        /// <param name="viewTypeToViewModelTypeResolver">The view type to view model type resolver.</param>
        public static void SetDefaultViewTypeToViewModelTypeResolver(Func<Type, Type> viewTypeToViewModelTypeResolver)
        {
            _defaultViewTypeToViewModelTypeResolver = viewTypeToViewModelTypeResolver;
        }

        /// <summary>
        /// Automatically looks up the viewmodel that corresponds to the current view, using two strategies:
        /// It first looks to see if there is a mapping registered for that view, if not it will fallback to the convention based
        /// approach.
        /// </summary>
        /// <param name="view">The dependency object, typically a view.</param>
        /// <param name="fqdn">The fqdn of the view model.</param>
        /// <param name="setDataContextCallback">The call back to use to create the binding between the View and ViewModel</param>
        public static void ViewModelFqdnChanged(object view, string fqdn, Action<object, object> setDataContextCallback)
        {
            if (string.IsNullOrEmpty(fqdn))
            {
                throw new ArgumentNullException(nameof(fqdn));
            }
            // Try mappings first            
            var viewModel = GetViewModelForFqdn(fqdn);
            // try to use ViewModel type
            if (viewModel == null)
            {
                //check type mappings               
                var viewModelType = GetViewModelTypeForView(view.GetType()) ?? FqdnViewTypeToViewModelTypeResolver(view.GetType(), fqdn);
                // fallback to convention based
                if (viewModelType == null)
                {
                    return;
                }
                viewModel = _defaultViewModelFactoryWithViewParameter != null ? _defaultViewModelFactoryWithViewParameter(view, viewModelType) : _defaultViewModelFactory(viewModelType);
            }
            setDataContextCallback(view, viewModel);
        }

        /// <summary>
        /// Gets the ViewModel type for the specified view.
        /// </summary>
        /// <param name="fqdn">The View that the ViewModel wants.</param>
        /// <returns>The ViewModel type that corresponds to the View.</returns>
        private static object GetViewModelForFqdn(string fqdn)
        {
            return TypeFactories.ContainsKey(fqdn) ? TypeFactories[fqdn] : null;
        }

        /// <summary>
        /// Gets the view model for the specified view.
        /// </summary>
        /// <param name="view">The view that the view model wants.</param>
        /// <returns>The ViewModel that corresponds to the view passed as a parameter.</returns>
        private static object GetViewModelForView(object view)
        {
            var viewKey = view.GetType().ToString();
            // Mapping of view models base on view type (or instance) goes here
            return Factories.ContainsKey(viewKey) ? Factories[viewKey]() : null;
        }

        /// <summary>
        /// Gets the ViewModel type for the specified view.
        /// </summary>
        /// <param name="view">The View that the ViewModel wants.</param>
        /// <returns>The ViewModel type that corresponds to the View.</returns>
        private static Type GetViewModelTypeForView(Type view)
        {
            var viewKey = view.ToString();
            return TypeFactories.ContainsKey(viewKey) ? TypeFactories[viewKey] : null;
        }

        #endregion
    }
}