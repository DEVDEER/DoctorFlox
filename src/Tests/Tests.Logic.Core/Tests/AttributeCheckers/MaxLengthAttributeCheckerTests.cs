namespace deveer.DoctorFlox.Tests.Logic.Core.Tests.AttributeCheckers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using BaseClasses;

    using devdeer.DoctorFlox.Helpers.Validation;

    using Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestModels;

    /// <summary>
    /// Contains unit tests for the type <see cref="MaxLengthAttributeChecker" />.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MaxLengthAttributeCheckerTests : BaseTest
    {
        #region methods

        /// <summary>
        /// Checks if the <see cref="MaxLengthAttributeChecker.InternalCheck" /> method reacts as expected.
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"DataSources\MaxLengthAttributeCheckerInternalCheckTest.csv")]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", "MaxLengthAttributeCheckerInternalCheckTest", "MaxLengthAttributeCheckerInternalCheckTest#csv", DataAccessMethod.Sequential)]
        public void MaxLengthAttributeCheckerInternalCheckTest()
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
                Lastname = GetConvertedTestValueAsString(5)
            };
            var checker = new MaxLengthAttributeChecker();
            var firstNameAttribute = item.GetAttribute<MaxLengthAttribute>(nameof(TestDataModel.Firstname));
            var lastNameAttribute = item.GetAttribute<MaxLengthAttribute>(nameof(TestDataModel.Lastname));
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