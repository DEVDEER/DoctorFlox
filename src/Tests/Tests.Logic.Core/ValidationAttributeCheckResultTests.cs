namespace deveer.DoctorFlox.Tests.Logic.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using devdeer.DoctorFlox.Models;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the type <see cref="ValidationAttributeCheckResult" />.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ValidationAttributeCheckResultTests
    {
        #region methods

        /// <summary>
        /// Tests if the constructor will store the provided values correctly.
        /// </summary>
        [TestMethod]
        public void ValidationAttributeCheckResultConstructorTest()
        {
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 1000; i++)
            {
                var desiredLength = random.Next(1, 20000);
                var errorMessage = new string('c', desiredLength);
                var item = new ValidationAttributeCheckResult(i % 10 == 0, errorMessage);
                Assert.AreEqual(i % 10 == 0, item.IsValid);
                Assert.IsNotNull(item.ErrorMessage);
                Assert.AreEqual(desiredLength, item.ErrorMessage.Length);
            }
        }

        #endregion
    }
}