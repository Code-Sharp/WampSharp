using System.Collections.Concurrent;
using System.Collections.Generic;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests.Rpc.Helpers
{
    public class MockWampRpcCallManager<TMessage>
    {
        private readonly IDictionary<string, MockWampRpcCallDetails<TMessage>> mCallIdToCallDetails =
            new ConcurrentDictionary<string, MockWampRpcCallDetails<TMessage>>();

        public IWampServer GetServer(IWampRpcClient<TMessage> client)
        {
            return new MockWampRpcServerProxy(this, client);
        }

        private void CallArrived(IWampRpcClient<TMessage> client, string callId, string procUri, object[] arguments)
        {
            mCallIdToCallDetails[callId] = new MockWampRpcCallDetails<TMessage>(client, callId, procUri, arguments);
        }

        public IEnumerable<MockWampRpcCallDetails<TMessage>> AllCalls => mCallIdToCallDetails.Values;

        public MockWampRpcCallDetails<TMessage> GetCallDetails(string callId)
        {

            if (mCallIdToCallDetails.TryGetValue(callId, out MockWampRpcCallDetails<TMessage> result))
            {
                return result;
            }

            return null;
        }

        private class MockWampRpcServerProxy : IWampServer
        {
            private readonly MockWampRpcCallManager<TMessage> mParent;
            private readonly IWampRpcClient<TMessage> mClient;

            public MockWampRpcServerProxy(MockWampRpcCallManager<TMessage> parent,
                                          IWampRpcClient<TMessage> client)
            {
                mParent = parent;
                mClient = client;
            }

            public void Prefix(IWampClient client, string prefix, string uri)
            {
            }

            public void Call(IWampClient client, string callId, string procUri, params object[] arguments)
            {
                mParent.CallArrived(mClient, callId, procUri, arguments);
            }

            public void Subscribe(IWampClient client, string topicUri)
            {
            }

            public void Unsubscribe(IWampClient client, string topicUri)
            {
            }

            public void Publish(IWampClient client, string topicUri, object @event)
            {
            }

            public void Publish(IWampClient client, string topicUri, object @event, bool excludeMe)
            {
            }

            public void Publish(IWampClient client, string topicUri, object @event, string[] exclude)
            {
            }

            public void Publish(IWampClient client, string topicUri, object @event, string[] exclude, string[] eligible)
            {
            }
        }
    }
}