using System;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Message;

namespace WampSharp.Core.Listener.V1
{
    /// <summary>
    /// A <see cref="WampListener{TMessage,TClient}"/> that is
    /// WAMPv1 specific.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampListener<TMessage> : WampListener<TMessage, IWampClient>
    {
    	public event EventHandler<WampSessionEventArgs> SessionCreated;
    	public event EventHandler<WampSessionEventArgs> SessionClosed;
    	//event EventHandler AfterMessageReceived;
    	public Action<string, string> CallInvoked;
    	
        /// <summary>
        /// Creates a new instance of <see cref="WampListener{TMessage}"/>
        /// </summary>
        /// <param name="listener">The <see cref="IWampConnectionListener{TMessage}"/> used in order to 
        /// accept incoming connections.</param>
        /// <param name="handler">The <see cref="IWampIncomingMessageHandler{TMessage,TClient}"/> used
        /// in order to dispatch incoming messages.</param>
        /// <param name="clientContainer">The <see cref="IWampClientContainer{TMessage,TClient}"/> use
        /// in order to store the connected clients.</param>
        public WampListener(IWampConnectionListener<TMessage> listener,
                            IWampIncomingMessageHandler<TMessage, IWampClient> handler,
                            IWampClientContainer<TMessage, IWampClient> clientContainer)
            : base(listener, handler, clientContainer)
        {
        }

		protected override void OnNewMessage(IWampConnection<TMessage> connection, WampSharp.Core.Message.WampMessage<TMessage> message)
		{
			base.OnNewMessage(connection, message);
			
			if ((message.MessageType == WampMessageType.v1Call) && (CallInvoked != null))
			{
				IWampClient client = ClientContainer.GetClient(connection);
				CallInvoked(client.SessionId, "");
			}
		}
		
        protected override void OnNewConnection(IWampConnection<TMessage> connection)
        {
            base.OnNewConnection(connection);

            IWampClient client = ClientContainer.GetClient(connection);

            client.Welcome(client.SessionId, 1, "WampSharp");
            
            var sessionCreated = SessionCreated;
            if (sessionCreated != null)
            	sessionCreated(this, new WampSessionEventArgs(client.SessionId));
        }
        
        protected override void OnCloseConnection(IWampConnection<TMessage> connection)
        {
        	var sessionClosed = SessionClosed;
            if (sessionClosed != null)
            {
				IWampClient client = ClientContainer.GetClient(connection);
				sessionClosed(this, new WampSessionEventArgs(client.SessionId));
            }
            
        	base.OnCloseConnection(connection);
        }
    }
}