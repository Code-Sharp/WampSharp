using System.Collections.Generic;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Cra;

namespace WampSharp.Tests.Dispatch.Helpers
{
    public class MockWampClient : IWampClient<MockRaw>
    {
        public void Welcome(string sessionId, int protocolVersion, string serverIdent)
        {
            throw new System.NotImplementedException();
        }

        public IDictionary<string, object> ClientContext => throw new System.NotImplementedException();

        public string SessionId => throw new System.NotImplementedException();

        public void CallResult(string callId, MockRaw result)
        {
            throw new System.NotImplementedException();
        }

        public void CallError(string callId, string errorUri, string errorDesc)
        {
            throw new System.NotImplementedException();
        }

        public void CallError(string callId, string errorUri, string errorDesc, MockRaw errorDetails)
        {
            throw new System.NotImplementedException();
        }

        public void Event(string topicUri, MockRaw @event)
        {
            throw new System.NotImplementedException();
        }

        public IWampCraAuthenticator CraAuthenticator
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
    }
}