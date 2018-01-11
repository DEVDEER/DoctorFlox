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
    /// Contains unit tests for the type <see cref="StringLengthAttributeChecker" />.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StringLengthAttributeCheckerTests : BaseTest
    {
        #region methods

        /// <summary>
        /// Checks if the <see cref="StringLengthAttributeChecker.InternalCheck" /> method reacts as expected.
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"DataSources\StringLengthAttributeCheckerInternalCheckTest.csv")]
        [DataSource(
            @"Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "StringLengthAttributeCheckerInternalCheckTest",
            "StringLengthAttributeCheckerInternalCheckTest#csv",
            DataAccessMethod.Sequential)]
        public void StringLengthAttributeCheckerInternalCheckTest()
        {
            // arrange
            var expectedFirstnameValid = GetConvertedTestValueAsBool(0) ?? throw new ApplicationException("Could not retrieve expected value.");
            var expectedFirstnameErrorMessageKey = GetConvertedTestValueAsString(1);
            var expectedLastnameValid = GetConvertedTestValueAsBool(2) ?? throw new ApplicationException("Could not retrieve expected value.");
            var expectedLastnameErrorMessageKey = GetConvertedTestValueAsString(3);
            var expectedFirstnameErrorMessage = string.IsNullOrEmpty(expectedFirstnameErrorMessageKey) ? string.Empty : TestResources.ResourceManager.GetString(expectedFirstnameErrorMessageKey);
            var expectedLastnameErrorMessage = string.IsNullOrEmpty(expectedLastnameErrorMessageKey) ? string.Empty : TestResources.ResourceManager.GetString(expectedLastnameErrorMessageKey);
            var item = new TestDataModel
            {
                Firstname = GetConvertedTestValueAsString(4),
                Lastname = GetConvertedTestValueAsString(5),
                Birthday = GetConvertedTestValueAsDateTime(6)
            };
            var checker = new StringLengthAttributeChecker();
            var firstNameAttribute = item.GetAttribute<StringLengthAttribute>(nameof(TestDataModel.Firstname));
            var lastNameAttribute = item.GetAttribute<StringLengthAttribute>(nameof(TestDataModel.Lastname));
            // act 
            var resultFirstname = checker.Check(firstNameAttribute, item.Firstname);
            var resultLastname = checker.Check(lastNameAttribute, item.Lastname);
            // assert
            Assert.AreEqual(expectedFirstnameValid, resultFirstname.IsValid, "The validity check for firstname failed.");
            Assert.AreEqual(expectedFirstnameErrorMessage, resultFirstname.ErrorMessage, "The error message for firstname is unexpected.");
            Assert.AreEqual(expectedLastnameValid, resultLastname.IsValid, "The validity check for lastname failed.");
            Assert.AreEqual(expectedLastnameErrorMessage, resultLastname.ErrorMessage, "The error message for lastname is unexpected.");
        }

        #endregion
    }
}