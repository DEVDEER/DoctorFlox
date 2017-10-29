namespace devdeer.DoctorFlox.Logic.Wpf.Enumerations
{
    using System;
    using System.Linq;

    /// <summary>
    /// Specifies on which thread a something should be called back.
    /// </summary>
    public enum ThreadCallbackOption
    {
        /// <summary>
        /// The call is done on the same thread on which the triggering code works.
        /// </summary>
        Sender = 0,

        /// <summary>
        /// The call is done asynchronously on a background thread.
        /// </summary>
        ThreadPool = 1,

        /// <summary>
        /// The call is done on the UI thread.
        /// </summary>
        UiThread = 2
    }
}