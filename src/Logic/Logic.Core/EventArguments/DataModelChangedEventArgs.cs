namespace devdeer.DoctorFlox.EventArguments
{
    using System;
    using System.Linq;

    /// <summary>
    /// Is used as an event arguments for events handling changes on properties of type <typeparamref name="TDataModel" />.
    /// </summary>
    /// <typeparam name="TDataModel">The type of the data model this type handles.</typeparam>
    public class DataModelChangedEventArgs<TDataModel> : EventArgs
        where TDataModel : BaseDataModel
    {
        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="oldValue">The value of the property in the calling type before the change happened.</param>
        /// <param name="newValue">The new or current value of the property in the calling type.</param>
        public DataModelChangedEventArgs(TDataModel oldValue, TDataModel newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion

        #region properties

        /// <summary>
        /// The new or current value of the property in the calling type.
        /// </summary>
        public TDataModel NewValue { get; }

        /// <summary>
        /// The value of the property in the current type before it changed.
        /// </summary>
        public TDataModel OldValue { get; }

        #endregion
    }
}