using System;
using System.Linq;

namespace devdeer.DoctorFlox.Logic.Wpf.Messages
{
    using System;
    using System.Linq;

    /// <summary>
    /// A message that allows passing typed <see cref="Data"/> within.
    /// </summary>
    /// <typeparam name="TData">The type for the data to send.</typeparam>
    public class DataMessage<TData> : Message
    {
        #region constructors and destructors

        public DataMessage(TData data)
        {
            Data = data;
        }

        #endregion

        #region properties

        /// <summary>
        /// The data to send.
        /// </summary>
        public TData Data { get; }

        #endregion
    }

    /// <summary>
    /// A message that allows passing typed <see cref="Data"/> within and defining 
    /// <typeparamref name="TSender"/> and <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TData">The type for the data to send.</typeparam>
    /// <typeparam name="TSender">The type of the sending element.</typeparam>
    /// <typeparam name="TTarget">The type of the receiving element.</typeparam>
    public class DataMessage<TSender, TTarget, TData> : Message<TSender, TTarget>
    {
        #region constructors and destructors

        public DataMessage(TData data)
        {
            Data = data;
        }

        #endregion

        #region properties

        /// <summary>
        /// The data to send.
        /// </summary>
        public TData Data { get; }

        #endregion
    }
}