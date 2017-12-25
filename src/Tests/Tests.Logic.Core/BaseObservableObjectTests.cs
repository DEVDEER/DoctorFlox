namespace deveer.DoctorFlox.Tests.Logic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using devdeer.DoctorFlox.Logic.Wpf;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestModels;

    /// <summary>
    /// Contains unit tests for the type <see cref="BaseObservableObject" />.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BaseObservableObjectTests
    {
        #region methods

        /// <summary>
        /// Checks if <see cref="BaseObservableObject.PropertyChanged" /> is fired after each
        /// real change of the <see cref="TestObservable.SomeProperty" />.
        /// </summary>
        /// <remarks>
        /// The property is changed more times than the event should be raised because we expect the
        /// logic to to raise the event when the current value equals the new one.
        /// </remarks>
        [TestMethod]
        public void BaseObservableObjectPropertyChangedRaisingTest()
        {
            // arrange
            var testObject = new TestObservable();
            var propertiesRaised = new List<string>();
            testObject.PropertyChanged += (s, e) =>
            {
                propertiesRaised.Add(e.PropertyName);
            };
            // act 
            testObject.SomeProperty = Guid.NewGuid().ToString();
            testObject.SomeProperty = null;
            testObject.SomeProperty = null;
            testObject.SomeProperty = string.Empty;
            // assert 
            Assert.AreEqual(3, propertiesRaised.Count);
        }

        #endregion
    }
}