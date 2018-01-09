namespace devdeer.DoctorFlox.Helpers.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Extensions;

    using Models;

    /// <summary>
    /// Validation checker for <see cref="StringLengthAttribute" />.
    /// </summary>
    public class StringLengthAttributeChecker : BaseAttributeChecker<StringLengthAttribute>
    {
        #region methods

        /// <inheritdoc />
        protected override ValidationAttributeCheckResult InternalCheck(StringLengthAttribute attribute, object currentValue)
        {
            if ((currentValue?.ToString().Length ?? 0) < attribute.MinimumLength || (currentValue?.ToString().Length ?? 0) > attribute.MaximumLength)
            {
                return new ValidationAttributeCheckResult(false, attribute.ResolveErrorMessage());
            }
            return new ValidationAttributeCheckResult();
        }

        #endregion
    }
}