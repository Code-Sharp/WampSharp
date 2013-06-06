using System;
using System.Linq;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Curie;
using WampSharp.Core.Serialization;

namespace WampSharp.Rpc.Server
{
    public class WampRpcServer<TMessage> : IWampAuxiliaryServer,
                                           IWampRpcServer<TMessage>
    {
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly IWampRpcMetadataCatalog mRpcMetadataCatalog;

        public WampRpcServer(IWampFormatter<TMessage> formatter,
            IWampRpcMetadataCatalog rpcMetadataCatalog)
        {
            mFormatter = formatter;
            mRpcMetadataCatalog = rpcMetadataCatalog;
        }

        public void Prefix(IWampClient client, string prefix, string uri)
        {
            IWampCurieMapper mapper = client as IWampCurieMapper;

            mapper.Map(prefix, uri);
        }

        public async void Call(IWampClient client, string callId, string procUri, params TMessage[] arguments)
        {
            procUri = ResolveUri(client, procUri);

            IWampRpcMethod method = mRpcMetadataCatalog.ResolveMethodByProcUri(procUri);

            try
            {
                object[] parameters =
                    arguments.Zip(method.Parameters,
                                  (argument, type) =>
                                  mFormatter.Deserialize(type, argument))
                             .ToArray();


                object result = await method.InvokeAsync(parameters);

                client.CallResult(callId, result);
            }
            catch (WampRpcCallException ex)
            {
                client.CallError(callId, ex.ErrorUri, ex.Message, ex.ErrorDetails);
            }
            catch (Exception ex)
            {
                client.CallError(callId, ex.HelpLink, ex.Message);
            }
        }

        private static string ResolveUri(IWampClient client, string procUri)
        {
            IWampCurieMapper mapper = client as IWampCurieMapper;

            Uri uri;
            if (!Uri.TryCreate(procUri, UriKind.Absolute, out uri))
            {
                procUri = mapper.ResolveCurie(procUri);
            }
            return procUri;
        }
    }
}