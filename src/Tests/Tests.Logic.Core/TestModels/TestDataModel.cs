namespace deveer.DoctorFlox.Tests.Logic.Core.TestModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using devdeer.DoctorFlox.Logic.Wpf;

    /// <summary>
    /// A type used in internal tests for <see cref="BaseDataModel" />.
    /// </summary>
    /// <remarks>
    /// Ensure to implement Fody.PropertyChanged or something similar to perform
    /// automatic implementation of <see cref="INotifyPropertyChanged" />.
    /// </remarks>
    public class TestDataModel : BaseDataModel
    {
        #region properties

        /// <summary>
        /// The ae calculated from <see cref="Birthday" /> if this is given or <c>null</c>.
        /// </summary>
        public int? Age => Birthday.HasValue ? (int)(DateTime.Now.Subtract(Birthday.Value).TotalDays / 365.2) : default(int?);

        /// <summary>
        /// The optional birth date.
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// The firstname of a person with 50 chars max.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }

        /// <summary>
        /// The lastname of a person with 50 chars max.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }

        /// <summary>
        /// A person that is related to this person.
        /// </summary>
        public TestDataModel Related { get; set; }

        /// <inheritdoc />
        protected override bool ValidateOnInstantiation => true;

        #endregion
    }
}