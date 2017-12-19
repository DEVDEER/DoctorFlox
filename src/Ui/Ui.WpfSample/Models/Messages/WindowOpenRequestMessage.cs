namespace devdeer.DoctorFlox.Ui.WpfSample.Models.Messages
{
    using System;
    using System.Linq;

    using Enumerations;

    /// <summary>
    /// Is sent by a view model to signal that it wants to open a certain window.
    /// </summary>
    public class WindowOpenRequestMessage
    {
        #region constructors and destructors

        /// <summary>
        /// Default constructor defining the immutable type.
        /// </summary>
        /// <param name="windowType">The type of window that the sender wants to be opened.</param>
        public WindowOpenRequestMessage(WindowType windowType)
        {
            WindowType = windowType;
        }

        #endregion

        #region properties

        /// <summary>
        /// Defines the type of window that the sender wants to be opened.
        /// </summary>
        public WindowType WindowType { get; }
        
        #endregion
    }
}