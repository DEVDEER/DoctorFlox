namespace devdeer.DoctorFlox.Models
{
    using System;
    using System.Linq;

    /// <summary>
    /// Defines the result retrieved by
    /// </summary>
    public class ValidationAttributeCheckResult
    {
        #region constructors and destructors

        /// <summary>
        /// Default constructor defining <see cref="IsValid" /> as <c>true</c> and <see cref="ErrorMessage" /> as
        /// <see cref="string.Empty" />.
        /// </summary>
        public ValidationAttributeCheckResult()
        {
            IsValid = true;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// Constructor for defining properties of immutable type directly.
        /// </summary>
        /// <param name="isValid">Indicates if the current value is valid.</param>
        /// <param name="errorMessage">The error message if the current value is invalid.</param>
        public ValidationAttributeCheckResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        #endregion

        #region properties

        /// <summary>
        /// Contains the underlaying error message coming from the attribute if <see cref="IsValid" /> is <c>false</c>.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Indicates if the check operation succeeded (the current value is valid).
        /// </summary>
        public bool IsValid { get; }

        #endregion
    }
}