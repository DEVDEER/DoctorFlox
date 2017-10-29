using System;
using System.Linq;

namespace devdeer.DoctorFlox.Logic.Wpf.Messages
{
    using System;
    using System.Linq;

    /// <summary>
    /// Base class for all messages broadcasted by the Messenger.
    /// You can create your own message types by extending this class.
    /// </summary>
    public class Message
    {
        #region constructors and destructors

        /// <summary>
        /// Initializes a new instance of the BaseMessage class.
        /// </summary>
        public Message()
        {
        }

        /// <summary>
        /// Initializes a new instance of the BaseMessage class.
        /// </summary>
        /// <param name="sender">The message's original sender.</param>
        public Message(object sender)
        {
            Sender = sender;
        }

        /// <summary>
        /// Initializes a new instance of the BaseMessage class.
        /// </summary>
        /// <param name="sender">The message's original sender.</param>
        /// <param name="target">
        /// The message's intended target. This parameter can be used
        /// to give an indication as to whom the message was intended for. Of course
        /// this is only an indication, amd may be null.
        /// </param>
        public Message(object sender, object target) : this(sender)
        {
            Target = target;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the message's sender.
        /// </summary>
        public object Sender { get; protected set; }

        /// <summary>
        /// Gets or sets the message's intended target. This property can be used
        /// to give an indication as to whom the message was intended for. Of course
        /// this is only an indication, amd may be null.
        /// </summary>
        public object Target { get; protected set; }

        #endregion
    }

    /// <summary>
    /// Base class for all messages broadcasted by the Messenger.
    /// You can create your own message types by extending this class.
    /// </summary>
    public class Message<TSender, TTarget> : Message
    {
        #region constructors and destructors

        /// <summary>
        /// Initializes a new instance of the BaseMessage class.
        /// </summary>
        public Message()
        {
        }

        /// <summary>
        /// Initializes a new instance of the BaseMessage class.
        /// </summary>
        /// <param name="sender">The message's original sender.</param>
        public Message(TSender sender)
        {
            Sender = sender;
        }

        /// <summary>
        /// Initializes a new instance of the BaseMessage class.
        /// </summary>
        /// <param name="sender">The message's original sender.</param>
        /// <param name="target">
        /// The message's intended target. This parameter can be used
        /// to give an indication as to whom the message was intended for. Of course
        /// this is only an indication, amd may be null.
        /// </param>
        public Message(TSender sender, TTarget target) : this(sender)
        {
            Target = target;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the message's sender.
        /// </summary>
        public new TSender Sender { get; protected set; }

        /// <summary>
        /// Gets or sets the message's intended target. This property can be used
        /// to give an indication as to whom the message was intended for. Of course
        /// this is only an indication, amd may be null.
        /// </summary>
        public new TTarget Target { get; protected set; }

        #endregion
    }
}