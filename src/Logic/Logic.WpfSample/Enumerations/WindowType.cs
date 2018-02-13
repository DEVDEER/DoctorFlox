namespace devdeer.DoctorFlox.Logic.WpfSample.Enumerations
{
    using System;
    using System.Linq;

    /// <summary>
    /// Defines possible window types for this assembly.
    /// </summary>
    public enum WindowType
    {
        /// <summary>
        /// Invalid window type.
        /// </summary>
        Unkown = 0,

        /// <summary>
        /// The <see cref="MainWindow" />.
        /// </summary>
        MainWindow = 1,

        /// <summary>
        /// The <see cref="ChildWindow" />.
        /// </summary>
        ChildWindow = 2,

        /// <summary>
        /// The <see cref="CollectionWindow" />.
        /// </summary>
        CollectionWindow = 3
    }
}