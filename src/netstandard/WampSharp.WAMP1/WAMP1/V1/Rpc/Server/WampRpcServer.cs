using System;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Core.Curie;

namespace WampSharp.V1.Rpc.Server
{
    /// <summary>
    /// An server-side implementation of <see cref="IWampRpcServer{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampRpcServer<TMessage> : IWampRpcServer<TMessage>
    {
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly IWampRpcMetadataCatalog mRpcMetadataCatalog;

        /// <summary>
        /// Initializes a new instance of <see cref="WampRpcServer{TMessage}"/>.
        /// </summary>
        /// <param name="formatter">The <see cref="IWampFormatter{TMessage}"/>
        /// used in order to deserialize method arguments.</param>
        /// <param name="rpcMetadataCatalog">The <see cref="IWampRpcMetadataCatalog"/>
        /// used in order to map calls to their corresponding methods.</param>
        public WampRpcServer(IWampFormatter<TMessage> formatter,
                             IWampRpcMetadataCatalog rpcMetadataCatalog)
        {
            mFormatter = formatter;
            mRpcMetadataCatalog = rpcMetadataCatalog;
        }

        public async void Call(IWampClient client, string callId, string procUri, params TMessage[] arguments)
        {
            procUri = ResolveUri(client, procUri);

            IWampRpcMethod method = mRpcMetadataCatalog.ResolveMethodByProcUri(procUri);

            try
            {
                WampRequestContext.Current = new WampRequestContext(client);

                object[] parameters =
                    arguments.Zip(method.Parameters,
                                  (argument, type) =>
                                  mFormatter.Deserialize(type, argument))
                             .ToArray();

                object result = await method.InvokeAsync(client, parameters).ConfigureAwait(false);
                client.CallResult(callId, result);
            }
            catch (Exception ex)
            {
                HandleException(client, callId, ex);
            }
            finally
            {
                WampRequestContext.Current = null;
            }
        }

        private static void HandleException(IWampClient client, string callId, Exception innerException)
        {

            if (innerException is WampRpcCallException callException)
            {
                HandleWampException(client, callId, callException);
            }
            else
            {
                HandleNonWampException(client, callId, innerException);
            }
        }

        private static void HandleNonWampException(IWampClient client, string callId, Exception ex)
        {
            client.CallError(callId, ex.HelpLink, ex.Message);
        }

        private static void HandleWampException(IWampClient client, string callId, WampRpcCallException callException)
        {
            client.CallError(callId, callException.ErrorUri, callException.Message, callException.ErrorDetails);
        }

        private static string ResolveUri(IWampClient client, string procUri)
        {
            IWampCurieMapper mapper = client as IWampCurieMapper;

            return mapper.Resolve(procUri);
        }
    }
}