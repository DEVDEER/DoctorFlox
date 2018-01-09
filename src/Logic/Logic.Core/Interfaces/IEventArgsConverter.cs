﻿namespace devdeer.DoctorFlox.Interfaces
{
    using System;
    using System.Linq;

    using Commands;

    /// <summary>
    /// The definition of the converter used to convert an EventArgs
    /// in the <see cref="EventToCommand" /> class, if the
    /// <see cref="EventToCommand.PassEventArgsToCommand" /> property is true.
    /// Set an instance of this class to the <see cref="EventToCommand.EventArgsConverter" />
    /// property of the EventToCommand instance.
    /// </summary>
    public interface IEventArgsConverter
    {
        #region methods

        /// <summary>
        /// The method used to convert the EventArgs instance.
        /// </summary>
        /// <param name="value">
        /// An instance of EventArgs passed by the
        /// event that the EventToCommand instance is handling.
        /// </param>
        /// <param name="parameter">
        /// An optional parameter used for the conversion. Use
        /// the <see cref="EventToCommand.EventArgsConverterParameter" /> property
        /// to set this value. This may be null.
        /// </param>
        /// <returns>The converted value.</returns>
        object Convert(object value, object parameter);

        #endregion
    }
}