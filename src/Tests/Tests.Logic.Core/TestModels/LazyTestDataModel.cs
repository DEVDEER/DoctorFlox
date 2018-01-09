namespace deveer.DoctorFlox.Tests.Logic.Core.TestModels
{
    using System;
    using System.Linq;

    using devdeer.DoctorFlox;

    /// <summary>
    /// Can be used by tests to get a <see cref="BaseDataModel" /> which has
    /// no initial validation configured.
    /// </summary>
    public class LazyTestDataModel : TestDataModel
    {
        #region properties

        /// <inheritdoc />
        protected override bool ValidateOnInstantiation => false;

        #endregion
    }
}