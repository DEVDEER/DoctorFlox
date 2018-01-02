namespace devdeer.DoctorFlox.Logic.Wpf.Messages
{
    using System;
    using System.Linq;

    /// <summary>
    /// A message that allows passing typed <see cref="Data" /> within.
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
    /// A message that allows passing typed <see cref="Data" /> within and defining
    /// <typeparamref name="TSender" /> and <typeparamref name="TTarget" />.
    /// </summary>
    /// <typeparam name="TData">The type for the data to send.</typeparam>
    /// <typeparam name="TSender">The type of the sending element.</typeparam>
    /// <typeparam name="TTarget">The type of the receiving element.</typeparam>
    public class DataMessage<TSender, TTarget, TData> : Message<TSender, TTarget>
    {
        #region constructors and destructors

        /// <summary>
        /// Initializes a new instance passing in <paramref name="sender" /> and <paramref name="data" />.
        /// </summary>
        /// <param name="sender">The message's original sender.</param>
        /// <param name="data">The data to send.</param>
        public DataMessage(TSender sender, TData data) : base(sender)
        {
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance passing in <paramref name="sender" /> and <paramref name="data" />.
        /// </summary>
        /// <param name="sender">The message's original sender.</param>
        /// <param name="target">The message`s target.</param>
        /// <param name="data">The data to send.</param>
        public DataMessage(TSender sender, TTarget target, TData data) : base(sender, target)
        {
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance passing in <paramref name="data" /> only.
        /// </summary>
        /// <param name="data">The data to send.</param>
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