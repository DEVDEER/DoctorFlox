namespace devdeer.DoctorFlox.Interfaces
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Models;

    /// <summary>
    /// Non-generic interface defintion for reflection purposes.
    /// </summary>
    public interface IValidationAttributeChecker
    {
        #region methods

        /// <summary>
        /// Performs the actual value checking.
        /// </summary>
        /// <param name="attribute">The attribute that is applied to the originating property.</param>
        /// <param name="currentValue">The value stored in the property to check.</param>
        /// <returns>The result containing a success flag and an error message to display in the client.</returns>
        ValidationAttributeCheckResult Check(object attribute, object currentValue);

        #endregion
    }

    /// <summary>
    /// Must be implemented by all types that support handling of <see cref="ValidationAttribute" />.
    /// </summary>
    /// <typeparam name="TAtt">The type of the <see cref="Attribute" /> which is checked by this checker.</typeparam>
    public interface IValidationAttributeChecker<in TAtt> : IValidationAttributeChecker
        where TAtt : ValidationAttribute
    {
        #region methods

        /// <summary>
        /// Performs the actual value checking.
        /// </summary>
        /// <param name="attribute">The attribute that is applied to the originating property.</param>
        /// <param name="currentValue">The value stored in the property to check.</param>
        /// <returns>The result containing a success flag and an error message to display in the client.</returns>
        ValidationAttributeCheckResult Check(TAtt attribute, object currentValue);

        #endregion
    }
}