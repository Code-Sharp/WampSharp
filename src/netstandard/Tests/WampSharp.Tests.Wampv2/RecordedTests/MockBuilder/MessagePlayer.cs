using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Message;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public abstract class MessagePlayer<TMessage> : IMessagePlayer<TMessage>
    {
        private readonly IWampIncomingMessageHandler<TMessage, IWampClientProxy<TMessage>> mServerPipe;
        protected readonly IEnumerable<WampMessage<TMessage>> mMessages;
        private readonly WampMessageType[] mCategories;

        public MessagePlayer(IEnumerable<WampMessage<TMessage>> messages,
                             WampMessageType[] categories,
                             IWampIncomingMessageHandler<TMessage, IWampClientProxy<TMessage>> handler)
        {
            mMessages = messages;
            mCategories = categories;
            mServerPipe = handler;
        }

        public IWampClientProxy<TMessage> Client { get; set; }

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