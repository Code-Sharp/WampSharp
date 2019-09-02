using System.Reflection;
using WampSharp.Core.Proxy;
using WampSharp.Core.Utilities;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Core.Listener.ClientBuilder
{
    internal abstract class WampClientProxyBase : ProxyBase
    {
        private static readonly MethodInfo mWelcome = Method.Get((IWampClient proxy) => proxy.Welcome(default, default, default));
        private static readonly MethodInfo mCallError3 = Method.Get((IWampClient proxy) => proxy.CallError(default, default, default));
        private static readonly MethodInfo mCallError4 = Method.Get((IWampClient proxy) => proxy.CallError(default, default, default, default));
        private static readonly MethodInfo mCallResult = Method.Get((IWampClient proxy) => proxy.CallResult(default, default));
        private static readonly MethodInfo mEvent = Method.Get((IWampClient proxy) => proxy.Event(default, default));

        public WampClientProxyBase(IWampOutgoingMessageHandler messageHandler, IWampOutgoingRequestSerializer requestSerializer)
            : base(messageHandler, requestSerializer)
        {
        }

        public void Welcome(string sessionId, int protocolVersion, string serverIdent)
        {
            Send(mWelcome, sessionId, protocolVersion, serverIdent);
        }

        public void CallResult(string callId, object result)
        {
            Send(mCallResult, callId, result);
        }

        public void CallError(string callId, string errorUri, string errorDesc)
        {
            Send(mCallError3, callId, errorUri, errorDesc);
        }

        public void CallError(string callId, string errorUri, string errorDesc, object errorDetails)
        {
            Send(mCallError4, callId, errorUri, errorDesc, errorDetails);
        }

        public void Event(string topicUri, object @event)
        {
            Send(mEvent, topicUri, @event);
        }
    }
}