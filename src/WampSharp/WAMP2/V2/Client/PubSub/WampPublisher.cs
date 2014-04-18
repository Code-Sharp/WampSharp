using System;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal class WampPublisher<TMessage> : IWampTopicPublicationProxy,
        IWampPublisher<TMessage>
    {
        public WampPublisher(IWampServerProxy proxy)
        {
        }

        public Task<long> Publish(string topicUri, object options)
        {
            throw new NotImplementedException();
        }

        public Task<long> Publish(string topicUri, object options, object[] arguments)
        {
            throw new NotImplementedException();
        }

        public Task<long> Publish(string topicUri, object options, object[] arguments, object argumentKeywords)
        {
            throw new NotImplementedException();
        }

        public void Published(long requestId, long publicationId)
        {
            throw new NotImplementedException();
        }
    }
}