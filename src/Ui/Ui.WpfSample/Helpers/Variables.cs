namespace devdeer.DoctorFlox.Ui.WpfSample.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;

    using Models;

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

        /// <summary>
        /// Serves as a factory for groups.
        /// </summary>
        public static IEnumerable<GroupDataModel> Groups { get; } = new[]
        {
            new GroupDataModel
            {
                Label = "Group 1"
            },
            new GroupDataModel
            {
                Label = "Group 2"
            },
            new GroupDataModel
            {
                Label = "Group 3"
            }
        };

        #endregion
    }
}