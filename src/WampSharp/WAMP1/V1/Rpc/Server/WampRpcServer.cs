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

#if !NET45
        public void Call(IWampClient client, string callId, string procUri, params TMessage[] arguments)
#else
        public async void Call(IWampClient client, string callId, string procUri, params TMessage[] arguments)
#endif
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

#if !NET45
                InnerCall(client, callId, method.InvokeAsync(parameters));
            }
#else
                object result = await method.InvokeAsync(parameters);
                client.CallResult(callId, result);
            }
            catch (WampRpcCallException ex)
            {
                client.CallError(callId, ex.ErrorUri, ex.Message, ex.ErrorDetails);
            }
#endif
            catch (Exception ex)
            {
                client.CallError(callId, ex.HelpLink, ex.Message);
            }
        }

#if !NET45
        private void InnerCall(IWampClient client, string callId, Task<object> task)
        {
            // I guess there is a prettier way.
            if (task.IsCompleted)
            {
                InnerCallCallback(client, callId, task);
            }
            else
            {
                task.ContinueWith
                    (x => InnerCallCallback(client, callId, x));                
            }

        }

        private static void InnerCallCallback(IWampClient client, string callId, Task<object> task)
        {
            if (task.Exception == null)
            {
                client.CallResult(callId, task.Result);
            }
            else
            {
                CallError(client, callId, task.Exception);
            }
        }

        // TODO: Don't repeat yourself: duplicated code that appears also in the framework 4.5 version
        // TODO: (Method Call)
        private static void CallError(IWampClient client, string callId, AggregateException taskException)
        {
            Exception innerException =
                taskException.InnerException;

            WampRpcCallException wampException =
                innerException as WampRpcCallException;

            if (wampException != null)
            {
                client.CallError(callId, wampException.ErrorUri, wampException.Message,
                                 wampException.ErrorDetails);
            }
            else
            {
                client.CallError(callId, innerException.HelpLink, innerException.Message);
            }
        }
#endif

        private static string ResolveUri(IWampClient client, string procUri)
        {
            IWampCurieMapper mapper = client as IWampCurieMapper;

            return mapper.Resolve(procUri);
        }
    }
}