using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests.Dispatch.Helpers
{
    public class MockWampServer : IWampServer<MockRaw>
    {
        public void Prefix(IWampClient client, string prefix, string uri)
        {
        }

        public void Call(IWampClient client, string callId, string procUri, params MockRaw[] arguments)
        {
        }

        public void Subscribe(IWampClient client, string topicUri)
        {
        }

        public void Unsubscribe(IWampClient client, string topicUri)
        {
        }

        public void Publish(IWampClient client, string topicUri, MockRaw @event)
        {
        }

        public void Publish(IWampClient client, string topicUri, MockRaw @event, bool excludeMe)
        {
        }

        public void Publish(IWampClient client, string topicUri, MockRaw @event, string[] exclude)
        {
        }

        public void Publish(IWampClient client, string topicUri, MockRaw @event, string[] exclude, string[] eligible)
        {
        }
    }
}