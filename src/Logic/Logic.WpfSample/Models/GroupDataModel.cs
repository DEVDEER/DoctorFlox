namespace devdeer.DoctorFlox.Logic.WpfSample.Models
{
    using System;
    using System.Linq;

    /// <summary>
    /// A wrapper for the data used in the PickGroupWindow.
    /// </summary>
    public class GroupDataModel : BaseDataModel

    {
        #region methods

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is GroupDataModel converted))
            {
                return false;
            }
            return converted.Id == Id;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return Id.GetHashCode() * 397;
            }
        }

        /// <summary>
        /// Compares this instance with <paramref name="other" />.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><c>true</c> if both ids are equal.</returns>
        protected bool Equals(GroupDataModel other)
        {
            return Id == other.Id;
        }

        #endregion

        #region properties

        /// <summary>
        /// The unique id of this group.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// A free label applied to this group.
        /// </summary>
        public string Label { get; set; }

        #endregion
    }
}