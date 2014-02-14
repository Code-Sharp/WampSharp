using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1
{
    public class DefaultWampServer<TMessage> : IWampServer<TMessage>
    {
        private readonly IWampRpcServer<TMessage> mRpcServer;
        private readonly IWampPubSubServer<TMessage> mPubSubServer;
        private readonly IWampAuxiliaryServer mAuxiliaryServer;

        public DefaultWampServer(IWampRpcServer<TMessage> rpcServer,
                                 IWampPubSubServer<TMessage> pubSubServer = null,
                                 IWampAuxiliaryServer auxiliaryServer = null)
        {
            mRpcServer = rpcServer;
            mPubSubServer = pubSubServer;
            mAuxiliaryServer = auxiliaryServer;
        }

        public void Prefix(IWampClient client, string prefix, string uri)
        {
            mAuxiliaryServer.Prefix(client, prefix, uri);
        }

        public void Call(IWampClient client, string callId, string procUri, params TMessage[] arguments)
        {
            mRpcServer.Call(client, callId, procUri, arguments);
        }

        public void Subscribe(IWampClient client, string topicUri)
        {
            mPubSubServer.Subscribe(client, topicUri);
        }

        public void Unsubscribe(IWampClient client, string topicUri)
        {
            mPubSubServer.Unsubscribe(client, topicUri);
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event)
        {
            mPubSubServer.Publish(client, topicUri, @event);
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, bool excludeMe)
        {
            mPubSubServer.Publish(client, topicUri, @event, excludeMe);
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude)
        {
            mPubSubServer.Publish(client, topicUri, @event, exclude);
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude, string[] eligible)
        {
            mPubSubServer.Publish(client, topicUri, @event, exclude, eligible);
        }
    }
}