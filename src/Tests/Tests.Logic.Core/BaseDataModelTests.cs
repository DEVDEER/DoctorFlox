namespace deveer.DoctorFlox.Tests.Logic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using devdeer.DoctorFlox;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestModels;

    /// <summary>
    /// Contains unit tests for the type <see cref="BaseDataModel" />.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BaseDataModelTests
    {
        #region methods

        /// <summary>
        /// Tests if the correct amount of <see cref="BaseDataModel.ErrorsChanged" /> events are raised and if
        /// the model is valid after the necessary changes.
        /// </summary>
        [TestMethod]
        public void BaseDataModelErrorEventTest()
        {
            // arrange
            var testObject = new TestDataModel();
            var errorEventsRaised = new List<string>();
            testObject.ErrorsChanged += (s, e) =>
            {
                errorEventsRaised.Add(e.PropertyName);
            };
            // act 
            testObject.Firstname = Guid.NewGuid().ToString();
            testObject.Lastname = Guid.NewGuid().ToString();
            testObject.Key = "ABCDE";
            // assert 
            Assert.AreEqual(2, errorEventsRaised.Count);
            Assert.IsFalse(testObject.HasErrors);
            Assert.IsTrue(testObject.IsOk);
        }

        /// <summary>
        /// Tests if the sample model has the correct amount of errors right when it gets constructed.
        /// </summary>
        [TestMethod]
        public void BaseDataModelErrorsPresentOnInitTest()
        {
            // arrange
            const int ExpectedErrorsCount = 2;
            // act
            var testObject = new TestDataModel
            {
                Related = new TestDataModel()
            };
            // assert
            Assert.IsTrue(testObject.HasErrors);
            Assert.IsFalse(testObject.IsOk);
            Assert.AreEqual(ExpectedErrorsCount, testObject.ErrorsCount);
            Assert.AreEqual(ExpectedErrorsCount, testObject.Related.ErrorsCount);
        }

        /// <summary>
        /// Tests if the <see cref="BaseDataModel.ValidateOnInstantiation" /> property works for new instances.
        /// </summary>
        [TestMethod]
        public void BaseDataModelValidateOnInstantiationTest()
        {
            // arrange
            var objectWithValidation = new TestDataModel();
            var objectWithoutValidation = new LazyTestDataModel();
            // act & assert
            Assert.IsFalse(objectWithValidation.IsOk);
            Assert.IsTrue(objectWithValidation.HasErrors);
            Assert.IsTrue(objectWithoutValidation.IsOk);
            Assert.IsFalse(objectWithoutValidation.HasErrors);
        }

        #endregion
    }
}