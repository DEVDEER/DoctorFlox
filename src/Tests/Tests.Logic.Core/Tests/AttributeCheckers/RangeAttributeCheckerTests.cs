namespace deveer.DoctorFlox.Tests.Logic.Core.Tests.AttributeCheckers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using BaseTypes;

    using Core.Extensions;

    using devdeer.DoctorFlox.Helpers.Validation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestModels;

    /// <summary>
    /// Contains unit tests for the type <see cref="RangeAttributeChecker" />.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RangeAttributeCheckerTests : BaseTest
    {
        #region methods

        /// <summary>
        /// Checks if the <see cref="RangeAttributeChecker.InternalCheck" /> method reacts as expected.
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"DataSources\RangeAttributeCheckerInternalCheckTest.csv")]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", "RangeAttributeCheckerInternalCheckTest", "RangeAttributeCheckerInternalCheckTest#csv", DataAccessMethod.Sequential)]
        public void RangeAttributeCheckerInternalCheckTest()
        {
            // arrange
            var expectedValid = GetConvertedTestValueAsBool(0) ?? throw new ApplicationException("Could not retrieve expected value.");
            var expectedErrorMessageKey = GetConvertedTestValueAsString(1);
            var expectedErrorMessage = string.IsNullOrEmpty(expectedErrorMessageKey) ? string.Empty : TestResources.ResourceManager.GetString(expectedErrorMessageKey);
            var item = new TestDataModel
            {
                Salary = GetConvertedTestValueAsInt(2) ?? 0
            };
            var checker = new RangeAttributeChecker();
            var keyAttribute = item.GetAttribute<RangeAttribute>(nameof(TestDataModel.Salary));
            // act 
            var result = checker.Check(keyAttribute, item.Salary);
            // assert
            Assert.AreEqual(expectedValid, result.IsValid, "The validity check for salary failed.");
            Assert.AreEqual(expectedErrorMessage, result.ErrorMessage, "The error message for salary is unexpected.");
        }

        #endregion
    }
}