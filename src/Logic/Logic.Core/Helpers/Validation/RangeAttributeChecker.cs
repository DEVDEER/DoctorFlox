namespace devdeer.DoctorFlox.Helpers.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Extensions;

    using Models;

    /// <summary>
    /// Validation checker for <see cref="RangeAttribute" />.
    /// </summary>
    public class RangeAttributeChecker : BaseAttributeChecker<RangeAttribute>
    {
        #region methods

        /// <inheritdoc />
        protected override ValidationAttributeCheckResult InternalCheck(RangeAttribute attribute, object currentValue)
        {
            if (!(currentValue is IComparable convertedValue))
            {
                throw new InvalidOperationException("Cannot apply range logic to this type of property.");
            }
            if (convertedValue.CompareTo(attribute.Minimum) == -1 || convertedValue.CompareTo(attribute.Maximum) == 1)
            {
                // either value is smaller than the minimum or greater than the maximum
                return new ValidationAttributeCheckResult(false, attribute.ResolveErrorMessage());
            }
            return new ValidationAttributeCheckResult();
        }

        #endregion
    }
}