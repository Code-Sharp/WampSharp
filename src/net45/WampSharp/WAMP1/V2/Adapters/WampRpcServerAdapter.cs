using System;
using WampSharp.Core.Serialization;
using WampSharp.V1.Core.Curie;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;
using IWampClient = WampSharp.V1.Core.Contracts.IWampClient;

namespace WampSharp.V2.Adapters
{
    public class WampRpcServerAdapter<TMessage> : V1.Core.Contracts.IWampRpcServer<TMessage>
    {
        private readonly IWampRpcOperationCatalog mRpcCatalog;
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampRpcServerAdapter(IWampRpcOperationCatalog rpcCatalog, IWampFormatter<TMessage> formatter)
        {
            mRpcCatalog = rpcCatalog;
            mFormatter = formatter;
        }

        public void Call(IWampClient client, string callId, string procUri, params TMessage[] arguments)
        {
            string resolvedUri = ResolveUri(client, procUri);

            mRpcCatalog.Invoke(new RpcOperationCallback(client, callId), mFormatter, new InvocationDetails(), resolvedUri, arguments);
        }

        private static string ResolveUri(IWampClient client, string procUri)
        {
            IWampCurieMapper mapper = client as IWampCurieMapper;

            return mapper.Resolve(procUri);
        }

        private class RpcOperationCallback : IWampRawRpcOperationCallback
        {
            private readonly IWampClient mClient;
            private readonly string mCallId;

            public RpcOperationCallback(IWampClient client, string callId)
            {
                mClient = client;
                mCallId = callId;
            }

            public void Result<TResult>(IWampFormatter<TResult> formatter, TResult details)
            {
                SendResult(null);
            }

            public void Result<TResult>(IWampFormatter<TResult> formatter, TResult details, TResult[] arguments)
            {
                if (arguments.Length == 0)
                {
                    SendResult(null);
                }
                else if (arguments.Length == 1)
                {
                    SendResult(arguments[0]);
                }
                else
                {
                    Wamp2Error(new {details, arguments});
                }
            }

            public void Result<TResult>(IWampFormatter<TResult> formatter, TResult details, TResult[] arguments, TResult argumentsKeywords)
            {
                Wamp2Error(new { details, arguments, argumentsKeywords });
            }

            public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error)
            {
                SendError(error, details);
            }

            public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error, TResult[] arguments)
            {
                SendError(error, new {details, arguments});
            }

            public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error, TResult[] arguments, TResult argumentsKeywords)
            {
                SendError(error, new { details, arguments, argumentsKeywords });
            }

            private void SendResult(object result)
            {
                mClient.CallResult(mCallId, result);
            }

            private void SendError(string error, object details)
            {
                mClient.CallError(mCallId, error, "An error has occured, see details", details);
            }

            private void Wamp2Error(object details)
            {
                mClient.CallError(mCallId,
                                  "wamp2_feature",
                                  "Received arguments not supported by this protocol version (included in details)",
                                  details);
            }
        }
    }
}