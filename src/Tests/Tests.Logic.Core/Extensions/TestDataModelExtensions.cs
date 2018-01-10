namespace deveer.DoctorFlox.Tests.Logic.Core.Extensions
{
    using System;
    using System.Linq;

    using TestModels;

    /// <summary>
    /// Provides extension methods for the type <see cref="TestDataModel" />.
    /// </summary>
    public static class TestDataModelExtensions
    {
        #region methods

        /// <summary>
        /// Retrieves a custom attribute for the given <paramref name="item" /> searching for a property by it's
        /// <paramref name="propertyName" />.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute the caller searches for.</typeparam>
        /// <param name="item">The target instance.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The custom attribute or <c>null</c> if no matching attribute was found.</returns>
        public static T GetAttribute<T>(this TestDataModel item, string propertyName)
        {
            if (!(item.GetType().GetProperty(propertyName)?.GetCustomAttributes(typeof(T), true).FirstOrDefault() is T attribute))
            {
                throw new ApplicationException("No matching attribute found on test model.");
            }
            return attribute;
        }

        #endregion
    }
}