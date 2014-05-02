using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Message;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public abstract class MessagePlayer<TMessage> : IMessagePlayer<TMessage>
    {
        private readonly IWampIncomingMessageHandler<TMessage, IWampClient<TMessage>> mServerPipe;
        protected readonly IEnumerable<WampMessage<TMessage>> mMessages;
        private readonly WampMessageType[] mCategories;
        private IWampClient<TMessage> mClient;

        public MessagePlayer(IEnumerable<WampMessage<TMessage>> messages,
                             WampMessageType[] categories,
                             IWampIncomingMessageHandler<TMessage, IWampClient<TMessage>> handler)
        {
            mMessages = messages;
            mCategories = categories;
            mServerPipe = handler;
        }

        public IWampClient<TMessage> Client
        {
            get { return mClient; }
            set { mClient = value; }
        }

        public void Response(WampMessage<TMessage> message)
        {
            if (mCategories.Contains(message.MessageType))
            {
                WampMessage<TMessage> request = FindRequest(message);

                if (request == null)
                {
                    return;
                }

                IEnumerable<WampMessage<TMessage>> responses = FindResponses(message, request);

                foreach (var response in responses)
                {
                    mServerPipe.HandleMessage(Client, response);
                }
            }
        }

        protected abstract IEnumerable<WampMessage<TMessage>> FindResponses(WampMessage<TMessage> message,
                                                                            WampMessage<TMessage> request);

        protected abstract WampMessage<TMessage> FindRequest(WampMessage<TMessage> message);
    }
}