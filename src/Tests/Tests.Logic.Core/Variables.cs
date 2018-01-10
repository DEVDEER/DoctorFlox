namespace deveer.DoctorFlox.Tests.Logic.Core
{
    using System;
    using System.Linq;

    /// <summary>
    /// Holds assembly-wide variables.
    /// </summary>
    internal static class Variables
    {
        #region constants

        /// <summary>
        /// The maximum amount of chars a string-property can take normally.
        /// </summary>
        public const int MaxStringLengthInTestModels = 50;

        /// <summary>
        /// The minimum amount of chars a string-property should take normally if it's not nullable.
        /// </summary>
        public const int MinStringLengthInTestModels = 1;

        public const int SalaryMaxValue = 100000;

        public const int SalaryMinValue = 1000;

        #endregion
    }
}