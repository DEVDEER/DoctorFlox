namespace devdeer.DoctorFlox.Logic.Wpf.Attributes
{
    using System;
    using System.Linq;

    using Helpers;

    /// <summary>
    /// Can be used to decorate a view model with to give information about the associated window to the view model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AssociatedViewAttribute : Attribute

    {
        #region member vars

        private Type _viewType;
        private string _viewTypeName;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Constructor for defining the <paramref name="viewTypeName" />.
        /// </summary>
        /// <param name="viewTypeName">The name or full name of the type of the view.</param>
        public AssociatedViewAttribute(string viewTypeName)
        {
            ViewTypeName = viewTypeName;
        }

        /// <summary>
        /// Constructor for defining the <paramref name="viewType" />.
        /// </summary>
        /// <param name="viewType">The type of the view.</param>
        public AssociatedViewAttribute(Type viewType)
        {
            ViewType = viewType;
        }

        #endregion

        #region properties

        /// <summary>
        /// The type of the view if any was given or resolved by <see cref="ViewTypeName" />.
        /// </summary>
        public Type ViewType
        {
            get
            {
                if (_viewType == null && !string.IsNullOrEmpty(_viewTypeName))
                {
                    _viewType = ReflectionHelper.GetViewTypeByNameOrFullName(_viewTypeName);
                }
                return _viewType;
            }
            set
            {
                if (_viewType == value)
                {
                    return;
                }
                _viewType = value;
                _viewTypeName = _viewType.FullName;
            }
        }

        /// <summary>
        /// The name or full name of the type of the view.
        /// </summary>
        public string ViewTypeName
        {
            get
            {
                if (string.IsNullOrEmpty(_viewTypeName) && _viewType != null)
                {
                    _viewTypeName = _viewType.FullName;
                }
                return _viewTypeName;
            }
            set
            {
                if (_viewTypeName.Equals(value, StringComparison.Ordinal))
                {
                    return;
                }
                _viewTypeName = value;
                _viewType = ReflectionHelper.GetViewTypeByNameOrFullName(_viewTypeName);
            }
        }

        #endregion
    }
}