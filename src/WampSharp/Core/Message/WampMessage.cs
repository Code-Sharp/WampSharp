namespace WampSharp.Core.Message
{
    /// <summary>
    /// Represents a WAMP protocol message.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampMessage<TMessage>
    {
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