using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Core.Curie;
using WampSharp.V1.Cra;

namespace WampSharp.Tests.Rpc.Helpers
{
    public class MockClient : IWampClient, IWampCurieMapper
    {
        private readonly IDictionary<string, object> mCallResults =
            new ConcurrentDictionary<string, object>();

        private readonly IDictionary<string, CallErrorDetails> mCallErrors =
            new ConcurrentDictionary<string, CallErrorDetails>();

        public void Welcome(string sessionId, int protocolVersion, string serverIdent)
        {
        }

        public string SessionId { get; set; }

        public object GetResultByCallId(string callId)
        {

            if (mCallResults.TryGetValue(callId, out object result))
            {
                return result;
            }

            return null;
        }

        public CallErrorDetails GetCallErrorByCallId(string callId)
        {

            if (mCallErrors.TryGetValue(callId, out CallErrorDetails result))
            {
                return result;
            }

            return null;
        }

        public void CallResult(string callId, object result)
        {
            mCallResults[callId] = result;
        }

        public void CallError(string callId, string errorUri, string errorDesc)
        {
            mCallErrors[callId] =
                new CallErrorDetails(errorUri, errorDesc, null);
        }

        public void CallError(string callId, string errorUri, string errorDesc, object errorDetails)
        {
            mCallErrors[callId] =
                new CallErrorDetails(errorUri, errorDesc, errorDetails);
        }

        public void Event(string topicUri, object @event)
        {
        }

        public string Resolve(string curie)
        {
            return curie;
        }

        public void Map(string prefix, string uri)
        {
            throw new NotImplementedException();
        }

        public IWampCraAuthenticator CraAuthenticator { get; set; }
    }
}