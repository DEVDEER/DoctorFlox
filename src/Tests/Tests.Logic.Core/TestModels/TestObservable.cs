namespace deveer.DoctorFlox.Tests.Logic.Core.TestModels
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    using devdeer.DoctorFlox;

    /// <summary>
    /// A type used in internal tests for <see cref="BaseObservableObject" />.
    /// </summary>
    /// <remarks>
    /// Ensure to implement Fody.PropertyChanged or something similar to perform
    /// automatic implementation of <see cref="INotifyPropertyChanged" />.
    /// </remarks>
    internal class TestObservable : BaseObservableObject
    {
        #region properties

        /// <summary>
        /// A sample property to use in tests.
        /// </summary>
        public string SomeProperty { get; set; }

        #endregion
    }
}