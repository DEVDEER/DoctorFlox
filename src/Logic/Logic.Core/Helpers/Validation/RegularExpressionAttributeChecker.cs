namespace devdeer.DoctorFlox.Helpers.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Extensions;

    using Models;

    /// <summary>
    /// Validation checker for <see cref="RegularExpressionAttribute" />.
    /// </summary>
    public class RegularExpressionAttributeChecker : BaseAttributeChecker<RegularExpressionAttribute>
    {
        #region methods

        /// <inheritdoc />
        protected override ValidationAttributeCheckResult InternalCheck(RegularExpressionAttribute attribute, object currentValue)
        {
            var regex = new Regex(attribute.Pattern);
            if (!regex.IsMatch(currentValue.ToString()))
            {
                return new ValidationAttributeCheckResult(false, attribute.ResolveErrorMessage());
            }
            return new ValidationAttributeCheckResult();
        }

        #endregion
    }
}