namespace devdeer.DoctorFlox.Helpers.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Extensions;

    using Models;

    /// <summary>
    /// Validation checker for <see cref="MaxLengthAttribute" />.
    /// </summary>
    public class MaxLengthAttributeChecker : BaseAttributeChecker<MaxLengthAttribute>
    {
        #region methods

        /// <inheritdoc />
        protected override ValidationAttributeCheckResult InternalCheck(MaxLengthAttribute attribute, object currentValue)
        {
            if ((currentValue?.ToString().Length ?? 0) > attribute.Length)
            {
                return new ValidationAttributeCheckResult(false, attribute.ResolveErrorMessage());
            }
            return new ValidationAttributeCheckResult();
        }

        #endregion
    }
}