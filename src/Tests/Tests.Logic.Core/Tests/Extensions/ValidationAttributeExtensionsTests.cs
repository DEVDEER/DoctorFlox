namespace deveer.DoctorFlox.Tests.Logic.Core.Tests.Extensions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using devdeer.DoctorFlox.Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the type <see cref="ValidationAttributeExtensions" />.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ValidationAttributeExtensionsTests
    {
        #region methods

        /// <summary>
        /// Checks if an <see cref="InvalidOperationException" /> is thrown when the ResourceType is invalid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ResolveErrorMessageExceptionTest()
        {
            var attribute = new MaxLengthAttribute(10)
            {
                ErrorMessageResourceType = typeof(string),
                ErrorMessageResourceName = "MaxLengthErrorMessage"
            };
            attribute.ResolveErrorMessage();
        }

        /// <summary>
        /// Tests if <see cref="ValidationAttributeExtensions.ResolveErrorMessage" /> resolves error messages correctly
        /// from resources.
        /// </summary>
        [TestMethod]
        public void ResolveErrorMessageTest()
        {
            // arrange
            var expectedResourceMessage = TestResources.MaxLengthErrorMessage;
            var expectedStaticMessage = "Foo";
            var attribute = new MaxLengthAttribute(10)
            {
                ErrorMessageResourceType = typeof(TestResources),
                ErrorMessageResourceName = "MaxLengthErrorMessage"
            };
            // act && assert
            var currentMessage = attribute.ResolveErrorMessage();
            Assert.AreEqual(expectedResourceMessage, currentMessage);
            attribute.ErrorMessage = expectedStaticMessage;
            currentMessage = attribute.ResolveErrorMessage();
            Assert.AreEqual(expectedStaticMessage, currentMessage);
            attribute = new MaxLengthAttribute(10);
            Assert.IsNull(attribute.ResolveErrorMessage());
        }

        #endregion
    }
}