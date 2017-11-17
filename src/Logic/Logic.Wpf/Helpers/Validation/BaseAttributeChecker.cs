namespace devdeer.DoctorFlox.Logic.Wpf.Helpers.Validation
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Interfaces;

    using Models;

    /// <summary>
    /// Abstract base class for all attribute check logic components when <see cref="IDataErrorInfo"/> is used.
    /// </summary>
    /// <typeparam name="TAtt">The type of the <see cref="ValidationAttribute"/> to check.</typeparam>
    public abstract class BaseAttributeChecker<TAtt> : IValidationAttributeChecker<TAtt>
        where TAtt : ValidationAttribute
    {
        #region explicit interfaces

        /// <inheritdoc />
        public ValidationAttributeCheckResult Check(object attribute, object currentValue)
        {
            return InternalCheck((TAtt)attribute, currentValue);
        }

        /// <inheritdoc />
        public ValidationAttributeCheckResult Check(TAtt attribute, object currentValue)
        {
            return InternalCheck(attribute, currentValue);
        }

        #endregion

        #region methods

        /// <summary>
        /// Defines the logic for performing the check for the given <paramref name="attribute"/>.
        /// </summary>
        /// <param name="attribute">The attribute that is applied to the originating property.</param>
        /// <param name="currentValue">The value stored in the property to check.</param>
        /// <returns>The result containing a success flag and an error message to display in the client.</returns>
        protected abstract ValidationAttributeCheckResult InternalCheck(TAtt attribute, object currentValue);

        #endregion
    }
}