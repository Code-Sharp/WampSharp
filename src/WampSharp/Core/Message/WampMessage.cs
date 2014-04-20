namespace WampSharp.Core.Message
{
    /// <summary>
    /// Represents a WAMP protocol message.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampMessage<TMessage>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public WampMessage()
        {
        }

        /// <summary>
        /// Copy constructor for inherited classes.
        /// </summary>
        /// <param name="other"></param>
        protected WampMessage(WampMessage<TMessage> other)
        {
            MessageType = other.MessageType;
            Arguments = (TMessage[]) other.Arguments.Clone();
        }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        public WampMessageType MessageType
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the arguments of this message.
        /// </summary>
        public TMessage[] Arguments
        {
            get; 
            set;
        }
    }
}