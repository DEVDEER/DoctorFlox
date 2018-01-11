namespace deveer.DoctorFlox.Tests.Logic.Core.TestModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using devdeer.DoctorFlox;

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
        [Required(ErrorMessageResourceType = typeof(TestResources), ErrorMessageResourceName = "RequiredErrorMessage")]
        [MinLength(Variables.MinStringLengthInTestModels, ErrorMessageResourceType = typeof(TestResources), ErrorMessageResourceName = "MinLengthErrorMessage")]
        [MaxLength(Variables.MaxStringLengthInTestModels, ErrorMessageResourceType = typeof(TestResources), ErrorMessageResourceName = "MaxLengthErrorMessage")]
        [StringLength(
            Variables.MaxStringLengthInTestModels,
            MinimumLength = Variables.MinStringLengthInTestModels,
            ErrorMessageResourceType = typeof(TestResources),
            ErrorMessageResourceName = "StringLengthErrorMessage")]
        public string Firstname { get; set; }

        /// <summary>
        /// A random key for the person (5 chars uppercase).
        /// </summary>
        [RegularExpression("^[A-Z]{5}$", ErrorMessageResourceType = typeof(TestResources), ErrorMessageResourceName = "KeyErrorMessage")]
        public string Key { get; set; }

        /// <summary>
        /// The lastname of a person with 50 chars max.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(TestResources), ErrorMessageResourceName = "RequiredErrorMessage")]
        [MinLength(Variables.MinStringLengthInTestModels, ErrorMessageResourceType = typeof(TestResources), ErrorMessageResourceName = "MinLengthErrorMessage")]
        [MaxLength(Variables.MaxStringLengthInTestModels, ErrorMessageResourceType = typeof(TestResources), ErrorMessageResourceName = "MaxLengthErrorMessage")]
        [StringLength(
            Variables.MaxStringLengthInTestModels,
            MinimumLength = Variables.MinStringLengthInTestModels,
            ErrorMessageResourceType = typeof(TestResources),
            ErrorMessageResourceName = "StringLengthErrorMessage")]
        public string Lastname { get; set; }

        /// <summary>
        /// A person that is related to this person.
        /// </summary>
        public TestDataModel Related { get; set; }

        /// <summary>
        /// The monthly salary of this person.
        /// </summary>
        [Range(Variables.SalaryMinValue, Variables.SalaryMaxValue, ErrorMessageResourceType = typeof(TestResources), ErrorMessageResourceName = "SalaryErrorMessage")]
        public int Salary { get; set; }

        /// <inheritdoc />
        protected override bool ValidateOnInstantiation => true;

        #endregion
    }
}