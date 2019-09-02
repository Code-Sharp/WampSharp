using System.Reflection;
using WampSharp.Core.Proxy;
using WampSharp.Core.Utilities;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Core.Listener.ClientBuilder
{
    internal abstract class WampServerProxyBase : ProxyBase, IWampServer
    {
        private static readonly MethodInfo mCall = Method.Get((IWampServer proxy) => proxy.Call(default, default, default, default));
        private static readonly MethodInfo mPrefix = Method.Get((IWampServer proxy) => proxy.Prefix(default, default, default));
        private static readonly MethodInfo mPublish3 = Method.Get((IWampServer proxy) => proxy.Publish(default, default, default));
        private static readonly MethodInfo mPublish4Boolean = Method.Get((IWampServer proxy) => proxy.Publish(default, default, default, default(bool)));
        private static readonly MethodInfo mPublish4StringArray = Method.Get((IWampServer proxy) => proxy.Publish(default, default, default, default(string[])));
        private static readonly MethodInfo mPublish5 = Method.Get((IWampServer proxy) => proxy.Publish(default, default, default, default, default));
        private static readonly MethodInfo mSubscribe = Method.Get((IWampServer proxy) => proxy.Subscribe(default, default));
        private static readonly MethodInfo mUnsubscribe = Method.Get((IWampServer proxy) => proxy.Unsubscribe(default, default));

        public WampServerProxyBase(IWampOutgoingMessageHandler messageHandler, IWampOutgoingRequestSerializer requestSerializer) : base(messageHandler, requestSerializer)
        {
        }

        public void Prefix(IWampClient client, string prefix, string uri)
        {
            Send(mPrefix, client, prefix, uri);
        }

        public void Call(IWampClient client, string callId, string procUri, params object[] arguments)
        {
            Send(mCall, client, callId, procUri, arguments);
        }

        public void Subscribe(IWampClient client, string topicUri)
        {
            Send(mSubscribe, client, topicUri);
        }

        public void Unsubscribe(IWampClient client, string topicUri)
        {
            Send(mUnsubscribe, client, topicUri);
        }

        public void Publish(IWampClient client, string topicUri, object @event)
        {
            Send(mPublish3, client, topicUri, @event);
        }

        public void Publish(IWampClient client, string topicUri, object @event, bool excludeMe)
        {
            Send(mPublish4Boolean, client, topicUri, @event, excludeMe);
        }

        public void Publish(IWampClient client, string topicUri, object @event, string[] exclude)
        {
            Send(mPublish4StringArray, client, topicUri, @event, exclude);
        }

        public void Publish(IWampClient client, string topicUri, object @event, string[] exclude, string[] eligible)
        {
            Send(mPublish5, client, topicUri, @event, exclude, eligible);
        }
    }
}