namespace devdeer.DoctorFlox.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Extensions;

    /// <summary>
    /// Provides logic for handling tasks related to <see cref="IDataErrorInfo" />.
    /// </summary>
    public static class ErrorValidationHelper
    {
        #region methods

        /// <summary>
        /// Retrieves a dictionary for each validation error of the <paramref name="targetObject" /> where
        /// the key will be the name of the property and the value the corresponding error message.
        /// </summary>
        /// <param name="targetObject">The object for which to perform the validation.</param>
        /// <param name="collapseInnerDataErrors">
        /// Indicates if the result should only take the first error of properties or not
        /// (defaults to <c>true</c>).
        /// </param>
        /// <returns>The dictionary of error informations.</returns>
        public static Dictionary<string, IEnumerable<string>> CollectErrors(object targetObject, bool collapseInnerDataErrors = true)
        {
            if (targetObject == null)
            {
                throw new ArgumentNullException(nameof(targetObject));
            }
            var result = new Dictionary<string, List<string>>();
            var type = targetObject.GetType();
            // check properties which have direct annotation
            var properties = ReflectionHelper.GetPropertiesWithValidationAttribute(type);
            foreach (var prop in properties)
            {
                var currentValue = prop.GetValue(targetObject);
                foreach (var customAttribute in prop.GetCustomAttributes(false))
                {
                    // Each property can have multiple validation attributes assigned to it.
                    if (!ReflectionHelper.ValidationCheckers.ContainsKey(customAttribute.GetType()))
                    {
                        // This attribute is not implemented in any implementation of IValidationAttributeChecker so we can't
                        // check it.
                        continue;
                    }
                    // perform the attribute check                    
                    var checkerResult = ReflectionHelper.ValidationCheckers[customAttribute.GetType()].Check(customAttribute, currentValue);
                    if (!checkerResult.IsValid)
                    {
                        // The property has an invalid value.
                        if (result.ContainsKey(prop.Name))
                        {
                            // add this error to the property errors
                            result[prop.Name].Add(checkerResult.ErrorMessage);
                        }
                        else
                        {
                            // add the property to the results
                            result.Add(
                                prop.Name,
                                new List<string>
                                {
                                    checkerResult.ErrorMessage
                                });
                        }
                    }
                }
            }
            // check properties which types implement IDataErrorInfo
            properties = ReflectionHelper.GetPropertiesWithDataErrorInfo(type);
            foreach (var prop in properties)
            {
                var innerErrors = CollectErrors(prop.GetValue(targetObject) as IDataErrorInfo);
                if (!collapseInnerDataErrors)
                {
                    // caller wants to retrieve all inner errors
                    foreach (var innerError in innerErrors)
                    {
                        // Add each error to the outer errors taking the outer property name as a prefix.
                        result.Add(innerError.Key, innerErrors.Select(iv => iv.Value).ToList().Merge());
                    }
                }
                else
                {
                    // caller wants to retrieved only one inner error per property
                    if (innerErrors.Any())
                    {
                        // We will take the first error of the object and map it as if it is the only error
                        // on the property in our instance.
                        result.Add(prop.Name, innerErrors.First().Value.ToList());
                    }
                }
            }
            // convert and retrieve result
            return result.ToDictionary(r => r.Key, r => r.Value.AsEnumerable());
        }

        #endregion
    }
}