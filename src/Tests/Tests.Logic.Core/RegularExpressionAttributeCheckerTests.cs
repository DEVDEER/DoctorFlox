namespace deveer.DoctorFlox.Tests.Logic.Core
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using devdeer.DoctorFlox.Helpers.Validation;
    using devdeer.DoctorFlox.Models;

    using Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestModels;

    /// <summary>
    /// Contains unit tests for the type <see cref="RegularExpressionAttributeChecker" />.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegularExpressionAttributeCheckerTests : BaseTest
    {
        #region methods

        /// <summary>
        /// Checks if the <see cref="RegularExpressionAttributeChecker.InternalCheck"/> method reacts as expected.
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"DataSources\RegularExpressionAttributeCheckerInternalCheckTest.csv")]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", "RegularExpressionAttributeCheckerInternalCheckTest", "RegularExpressionAttributeCheckerInternalCheckTest#csv", DataAccessMethod.Sequential)]
        public void RegularExpressionAttributeCheckerInternalCheckTest()
        {
            // arrange
            var expectedValid = GetConvertedTestValueAsBool(0) ?? throw new ApplicationException("Could not retrieve expected value.");
            var expectedErrorMessageKey  = GetConvertedTestValueAsString(1);
            var expectedErrorMessage = string.IsNullOrEmpty(expectedErrorMessageKey) ? string.Empty : TestResources.ResourceManager.GetString(expectedErrorMessageKey);
            var item = new TestDataModel
            {
               Key = GetConvertedTestValueAsString(2)
            };
            var checker = new RegularExpressionAttributeChecker();
            var keyAttribute = item.GetAttribute<RegularExpressionAttribute>(nameof(TestDataModel.Key));            
            // act 
            var result = checker.Check(keyAttribute, item.Key);            
            // assert
            Assert.AreEqual(expectedValid, result.IsValid, "The validity check for key failed.");
            Assert.AreEqual(expectedErrorMessage, result.ErrorMessage, "The error message for key is unexpected.");            
        }

        #endregion
    }
}