namespace devdeer.DoctorFlox.Logic.Wpf.Helpers.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Extensions;

    using Models;

    /// <summary>
    /// Validation checker for <see cref="MinLengthAttribute" />.
    /// </summary>
    public class MinLengthAttributeChecker : BaseAttributeChecker<MinLengthAttribute>
    {
        #region methods

        /// <inheritdoc />
        protected override ValidationAttributeCheckResult InternalCheck(MinLengthAttribute attribute, object currentValue)
        {
            if ((currentValue?.ToString().Length ?? 0) < attribute.Length)
            {
                return new ValidationAttributeCheckResult(false, attribute.ResolveErrorMessage());
            }
            return new ValidationAttributeCheckResult();
        }

        #endregion
    }
}