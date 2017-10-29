namespace devdeer.DoctorFlox.Ui.WpfSample.Helpers
{
    using System;
    using System.Linq;

    using Autofac;

    /// <summary>
    /// Provides access to global variables in this assembly.
    /// </summary>
    internal static class Variables
    {
        #region properties

        /// <summary>
        /// The AutoFac container built in app.xaml.cs.
        /// </summary>
        public static IContainer AutoFacContainer { get; set; }

        #endregion
    }
}