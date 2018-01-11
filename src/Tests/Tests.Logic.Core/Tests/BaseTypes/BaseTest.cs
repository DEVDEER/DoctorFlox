namespace deveer.DoctorFlox.Tests.Logic.Core.Tests.BaseTypes
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Abstract base class for all tests for shared logic.
    /// </summary>
    [TestClass]
    public abstract class BaseTest
    {
        #region constants

        /// <summary>
        /// Special value for field in CSV input file which will be mapped to <c>null</c> string
        /// </summary>
        public const string Nullstring = "NULL";

        #endregion

        #region methods

        /// <summary>
        /// Converts field of data row to <see cref="bool" />, converting <see cref="Nullstring" /> to <c>null</c>
        /// </summary>
        /// <param name="index">Index within <see cref="DataRow" />.</param>
        /// <returns>The value as <see cref="bool" /> or <c>null</c>.</returns>
        public bool? GetConvertedTestValueAsBool(int index)
        {
            var data = GetConvertedTestValueAsString(index);
            return data == null ? default(bool?) :
                bool.TryParse(data, out var result) ? result : default(bool?);
        }

        /// <summary>
        /// Converts field of data row to <see cref="DateTime" />, converting <see cref="Nullstring" /> to <c>null</c>
        /// </summary>
        /// <param name="index">Index within <see cref="DataRow" />.</param>
        /// <returns>The value as <see cref="DateTime" /> or <c>null</c>.</returns>
        public DateTime? GetConvertedTestValueAsDateTime(int index)
        {
            var data = GetConvertedTestValueAsString(index);
            return data == null ? default(DateTime?) :
                DateTime.TryParse(data, out var result) ? result : default(DateTime?);
        }

        /// <summary>
        /// Converts field of data row to <see cref="DateTimeOffset" />, converting <see cref="Nullstring" /> to <c>null</c>
        /// </summary>
        /// <param name="index">Index within <see cref="DataRow" />.</param>
        /// <returns>The value as <see cref="DateTimeOffset" /> or <c>null</c>.</returns>
        public DateTimeOffset? GetConvertedTestValueAsDateTimeOffset(int index)
        {
            var data = GetConvertedTestValueAsString(index);
            return data == null ? default(DateTimeOffset?) :
                DateTimeOffset.TryParse(data, out var result) ? result : default(DateTimeOffset?);
        }

        /// <summary>
        /// Converts field of data row to <see cref="decimal" />, converting <see cref="Nullstring" /> to <c>null</c>
        /// </summary>
        /// <param name="index">Index within <see cref="DataRow" />.</param>
        /// <returns>The value as <see cref="decimal" /> or <c>null</c>.</returns>
        public decimal? GetConvertedTestValueAsDecimal(int index)
        {
            var data = GetConvertedTestValueAsString(index);
            return data == null ? default(decimal?) :
                decimal.TryParse(data, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : default(decimal?);
        }

        /// <summary>
        /// Converts field of data row to <see cref="int" />, converting <see cref="Nullstring" /> to <c>null</c>
        /// </summary>
        /// <param name="index">Index within <see cref="DataRow" />.</param>
        /// <returns>The value as <see cref="int" /> or <c>null</c>.</returns>
        public int? GetConvertedTestValueAsInt(int index)
        {
            var data = GetConvertedTestValueAsString(index);
            return data == null ? default(int?) :
                int.TryParse(data, out var result) ? result : default(int?);
        }

        /// <summary>
        /// Converts field of data row to <see cref="long" />, converting <see cref="Nullstring" /> to <c>null</c>
        /// </summary>
        /// <param name="index">Index within <see cref="DataRow" />.</param>
        /// <returns>The value as <see cref="long" /> or <c>null</c>.</returns>
        public long? GetConvertedTestValueAsLong(int index)
        {
            var data = GetConvertedTestValueAsString(index);
            return data == null ? default(long?) :
                long.TryParse(data, out var result) ? result : default(long?);
        }

        /// <summary>
        /// Converts field of data row to <see cref="string" />, converting <see cref="Nullstring" /> to <c>null</c>
        /// </summary>
        /// <param name="index">Index within <see cref="DataRow" />.</param>
        /// <returns>The value as <see cref="string" /> or <c>null</c>.</returns>
        public string GetConvertedTestValueAsString(int index)
        {
            var data = GetTestData(index)?.ToString();
            return data == Nullstring ? null : data;
        }

        /// <summary>
        /// Get field of data row.
        /// </summary>
        /// <param name="index">Index within <see cref="DataRow" /></param>
        /// <returns>The raw data from the row.</returns>
        protected object GetTestData(int index)
        {
            return TestContext.DataRow[index];
        }

        #endregion

        #region properties

        /// <summary>
        /// The test context to use.
        /// </summary>
        public TestContext TestContext { get; set; }

        #endregion
    }
}