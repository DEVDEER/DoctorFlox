namespace devdeer.DoctorFlox.Ui.WpfSample.Models.Messages
{
    using System;
    using System.Linq;

    using DoctorFlox.Messages;

    using Enumerations;

    /// <summary>
    /// Is sent by a view model to signal that it wants to open a certain window.
    /// </summary>
    public class WindowOpenRequestMessage : DataMessage<WindowType>
    {
        #region constructors and destructors

        public WindowOpenRequestMessage(WindowType data) : base(data)
        {
        }

        #endregion
    }
}