using System;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal class WampPublisher<TMessage> : IWampTopicPublicationProxy,
        IWampPublisher<TMessage>, IWampPublisherError<TMessage>,
        IWampClientConnectionErrorHandler
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

        public void OnConnectionError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void OnConnectionClosed()
        {
            throw new NotImplementedException();
        }
    }
}