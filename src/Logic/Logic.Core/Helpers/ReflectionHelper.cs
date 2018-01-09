namespace devdeer.DoctorFlox.Helpers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Windows;

    using Attributes;

    using Interfaces;

    /// <summary>
    /// Provides helper methods for reflection-related tasks.
    /// </summary>
    public static class ReflectionHelper
    {
        #region constants

        /// <summary>
        /// Lazy factory for retrieving a list of all currently available types which derive from <see cref="Window" /> in the
        /// <see cref="AppDomain" />.
        /// </summary>
        public static Lazy<IEnumerable<Type>> ViewTypeListFactory = new Lazy<IEnumerable<Type>>(
            () =>
            {
                var result = new List<Type>();
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    result.AddRange(assembly.GetTypes().Where(t => !t.IsAbstract && typeof(Window).IsAssignableFrom(t)));
                }
                return result;
            });

        /// <summary>
        /// The on-demand factory for <see cref="ValidationCheckers" />.
        /// </summary>
        private static readonly Lazy<Dictionary<Type, IValidationAttributeChecker>> ValidationCheckersLazy = new Lazy<Dictionary<Type, IValidationAttributeChecker>>(
            () =>
            {
                var result = new Dictionary<Type, IValidationAttributeChecker>();
                var checkerTypes = typeof(BaseViewModel).Assembly.GetTypes().Where(t => !t.IsAbstract && typeof(IValidationAttributeChecker).IsAssignableFrom(t));
                foreach (var checkerType in checkerTypes)
                {
                    var attributeType = checkerType.BaseType?.GenericTypeArguments.First();
                    if (attributeType != null)
                    {
                        result.Add(attributeType, Activator.CreateInstance(checkerType) as IValidationAttributeChecker);
                    }
                }
                return result;
            });

        #endregion

        #region methods

        /// <summary>
        /// Retrieves all properties of a given <paramref name="targetType" /> which are inherting from
        /// <see cref="BaseDataModel" />.
        /// </summary>
        /// <param name="targetType">The type to inspect.</param>
        /// <returns>The list of properties deriving from <see cref="BaseDataModel" />.</returns>
        public static IEnumerable<PropertyInfo> GetPropertiesInheritingFromBaseDataModel(Type targetType)
        {
            if (BaseDataModelProperties.ContainsKey(targetType))
            {
                // found it in the cache
                return BaseDataModelProperties[targetType];
            }
            // get all public non-static properties of the target type implementing IDataErrorInfo
            var result = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => typeof(BaseDataModel).IsAssignableFrom(p.PropertyType)).Distinct().ToList();
            // update the cache
            BaseDataModelProperties.TryAdd(targetType, result);
            // return result 
            return result;
        }

        /// <summary>
        /// Retrieves all properties of a given <paramref name="targetType" /> which are flagged with at least one
        /// <see cref="IDataErrorInfo" />.
        /// </summary>
        /// <param name="targetType">The type to inspect.</param>
        /// <returns>The list of properties attributed with at least one <see cref="ValidationAttribute" />.</returns>
        public static IEnumerable<PropertyInfo> GetPropertiesWithDataErrorInfo(Type targetType)
        {
            if (DataErrorInfoProperties.ContainsKey(targetType))
            {
                // found it in the cache
                return DataErrorInfoProperties[targetType];
            }
            // get all public non-static properties of the target type implementing IDataErrorInfo
            var result = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => typeof(IDataErrorInfo).IsAssignableFrom(p.PropertyType)).Distinct().ToList();
            // update the cache
            DataErrorInfoProperties.TryAdd(targetType, result);
            // return result 
            return result;
        }

        /// <summary>
        /// Retrieves all properties of a given <paramref name="targetType" /> which are flagged with at least one
        /// <see cref="ValidationAttribute" />.
        /// </summary>
        /// <param name="targetType">The type to inspect.</param>
        /// <returns>The list of properties attributed with at least one <see cref="ValidationAttribute" />.</returns>
        public static IEnumerable<PropertyInfo> GetPropertiesWithValidationAttribute(Type targetType)
        {
            if (AnnotatedProperties.ContainsKey(targetType))
            {
                // found it in the cache
                return AnnotatedProperties[targetType];
            }
            // get all public non-static properties of the target type
            var instanceProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            // search for all attributes on properties distincted so that each property info will occur only once
            var result = (from prop in instanceProperties from attr in ValidationAttributeTypesLazy.Value where prop.IsDefined(attr, true) select prop).Distinct().ToList();
            // update the cache
            AnnotatedProperties.TryAdd(targetType, result);
            // return result 
            return result;
        }

        /// <summary>
        /// Tries to retrieve the type of a view by searching for the given <paramref name="nameOrFullName" /> inside
        /// the <see cref="ViewTypeListFactory" /> interpreting the <paramref name="nameOrFullName" /> as the types name or full
        /// name.
        /// </summary>
        /// <param name="nameOrFullName">The name of full name of the type of the view which should be returned.</param>
        /// <returns>The type of the view or <c>null</c> if the name wasn't found.</returns>
        public static Type GetViewTypeByNameOrFullName(string nameOrFullName)
        {
            if (string.IsNullOrEmpty(nameOrFullName))
            {
                return null;
            }
            return ViewTypeListFactory.Value.FirstOrDefault(t => t.Name.Equals(nameOrFullName, StringComparison.Ordinal) || (t.FullName?.Equals(nameOrFullName, StringComparison.Ordinal) ?? false));
        }

        /// <summary>
        /// Tries to find and retrieve at least one <see cref="AssociatedViewAttribute" /> assigned to the given
        /// <paramref name="targetType" />
        /// </summary>
        /// <param name="targetType">The type to inspect.</param>
        /// <returns>The first assigned attribute or <c>null</c> if none is assigned.</returns>
        public static AssociatedViewAttribute GetViewTypeNameAttribute(Type targetType)
        {
            return targetType.GetCustomAttributes(typeof(AssociatedViewAttribute)).Cast<AssociatedViewAttribute>().FirstOrDefault();
        }

        #endregion

        #region properties

        /// <summary>
        /// A dictionary where the key is some type and the value is the collection of properties which are flagged with at least
        /// on <see cref="ValidationAttribute" />.
        /// </summary>
        public static Dictionary<Type, IValidationAttributeChecker> ValidationCheckers => ValidationCheckersLazy.Value;

        /// <summary>
        /// A dictionary where the key is some type and the value is the collection of properties which are flagged with at least
        /// on <see cref="ValidationAttribute" />.
        /// </summary>
        private static ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> AnnotatedProperties { get; } = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        /// <summary>
        /// A dictionary where the key is some type and the value is the collection of properties which are inherting from
        /// <see cref="BaseDataModel" />.
        /// </summary>
        private static ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> BaseDataModelProperties { get; } = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        /// <summary>
        /// A dictionary where the key is some type and the value is the collection of properties which are implementing
        /// <see cref="IDataErrorInfo" />.
        /// </summary>
        private static ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> DataErrorInfoProperties { get; } = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        /// <summary>
        /// Is used internally to get all types inherting from <see cref="ValidationAttribute" /> only once if needed.
        /// </summary>
        private static Lazy<IEnumerable<Type>> ValidationAttributeTypesLazy =>
            new Lazy<IEnumerable<Type>>(
                () =>
                {
                    return typeof(ValidationAttribute).Assembly.GetTypes().Where(t => !t.IsAbstract && typeof(ValidationAttribute).IsAssignableFrom(t)).ToList();
                });

        #endregion
    }
}