namespace devdeer.DoctorFlox.Extensions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Resources;

    /// <summary>
    /// Provides extensions methods for the type <see cref="ValidationAttribute" />.
    /// </summary>
    public static class ValidationAttributeExtensions
    {
        #region methods

        /// <summary>
        /// Retrieves either the error message directly or the text coming from the resources.
        /// </summary>
        /// <param name="attribute">The validation attribute which defines the message or resource name.</param>
        /// <returns>Either the message, the resolved message or <c>null</c> if the resource was not found.</returns>
        /// <exception cref="InvalidOperationException">
        /// Is thrown if the resource mananger could not be instantiated using the
        /// provided resource type.
        /// </exception>
        public static string ResolveErrorMessage(this ValidationAttribute attribute)
        {
            if (!string.IsNullOrEmpty(attribute.ErrorMessage))
            {
                // this is clear -> just return the message
                return attribute.ErrorMessage;
            }
            if (string.IsNullOrEmpty(attribute.ErrorMessageResourceName) || attribute.ErrorMessageResourceType == null)
            {
                // we can't do anything because there is no ressource name given
                return null;
            }
            var manager = new ResourceManager(attribute.ErrorMessageResourceType);
            if (manager == null)
            {
                throw new InvalidOperationException("Resource type could not be used.");
            }
            // try to return the resource based text            
            return manager.GetString(attribute.ErrorMessageResourceName);
        }

        #endregion
    }
}