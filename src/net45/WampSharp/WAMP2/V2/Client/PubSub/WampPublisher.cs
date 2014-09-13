using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal class WampPublisher<TMessage> : IWampTopicPublicationProxy,
        IWampPublisher<TMessage>, IWampPublisherError<TMessage>
    {
        public WampPublisher(IWampServerProxy proxy, IWampClientConnectionMonitor monitor)
        {
        }

        public void OnConnectionError(Exception exception)
        {
        }

        public void OnConnectionClosed()
        {
        }

        public Task<long> Publish(string topicUri, PublishOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<long> Publish(string topicUri, PublishOptions options, object[] arguments)
        {
            throw new NotImplementedException();
        }

        public Task<long> Publish(string topicUri, PublishOptions options, object[] arguments, IDictionary<string, object> argumentKeywords)
        {
            throw new NotImplementedException();
        }

        public void Published(long requestId, long publicationId)
        {
            throw new NotImplementedException();
        }

        public void PublishError(long requestId, TMessage details, string error)
        {
            throw new NotImplementedException();
        }

        public void PublishError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            throw new NotImplementedException();
        }

        public void PublishError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            throw new NotImplementedException();
        }
    }
}