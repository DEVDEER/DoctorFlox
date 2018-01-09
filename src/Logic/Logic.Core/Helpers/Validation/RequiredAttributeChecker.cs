namespace devdeer.DoctorFlox.Helpers.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Extensions;

    using Models;

    /// <summary>
    /// Validation checker for <see cref="RequiredAttribute" />.
    /// </summary>
    public class RequiredAttributeChecker : BaseAttributeChecker<RequiredAttribute>
    {
        #region methods

        /// <inheritdoc />
        protected override ValidationAttributeCheckResult InternalCheck(RequiredAttribute attribute, object currentValue)
        {
            if (string.IsNullOrEmpty(currentValue?.ToString() ?? string.Empty))
            {
                return new ValidationAttributeCheckResult(false, attribute.ResolveErrorMessage());
            }
            return new ValidationAttributeCheckResult();
        }

        #endregion
    }
}