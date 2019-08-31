using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Logging;
using WampSharp.Core.Dispatch.Handler;

using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;

namespace WampSharp.Core.Proxy
{
    /// <summary>
    /// An implementation of <see cref="IWampOutgoingRequestSerializer"/>.
    /// </summary>
    public class WampOutgoingRequestSerializer<TMessage> : IWampOutgoingRequestSerializer
    {
        private readonly IDictionary<MethodInfo, WampMethodInfo> mMethodToWampMethod =
            new SwapDictionary<MethodInfo, WampMethodInfo>();

        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly ILog mLogger = LogProvider.GetLogger(typeof(WampOutgoingRequestSerializer<TMessage>));

        /// <summary>
        /// Initializes a new instance of <see cref="WampOutgoingRequestSerializer{TMessage}"/>.
        /// </summary>
        /// <param name="formatter">The <see cref="IWampFormatter{TMessage}"/> to
        /// serialize arguments with.</param>
        public WampOutgoingRequestSerializer(IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
        }

        public WampMessage<object> SerializeRequest(MethodInfo method, object[] arguments)
        {
            mLogger.DebugFormat("Calling remote peer proxy method: {Method}", method);

            WampMethodInfo wampMethod = GetWampMethod(method);

            WampMessageType messageType = wampMethod.MessageType;

            WampMessage<object> result = new WampMessage<object>()
                                               {
                                                   MessageType = messageType
                                               };

            List<object> argumentsToSerialize = arguments.ToList();

            if (wampMethod.HasWampClientArgument)
            {
                argumentsToSerialize.RemoveAt(0);
            }

            object[] paramsArgument = null;

            if (wampMethod.HasParamsArgument)
            {
                paramsArgument = (object[])argumentsToSerialize.Last();
                argumentsToSerialize.RemoveAt(argumentsToSerialize.Count - 1);
            }

            List<object> messageArguments = argumentsToSerialize.ToList();

            if (wampMethod.HasParamsArgument)
            {
                messageArguments.AddRange(paramsArgument);
            }

            result.Arguments = messageArguments.ToArray();

            return result;
        }

        private WampMethodInfo GetWampMethod(MethodInfo method)
        {

            if (mMethodToWampMethod.TryGetValue(method, out WampMethodInfo result))
            {
                return result;
            }
            else
            {
                result = new WampMethodInfo(method);
                mMethodToWampMethod[method] = result;
            }

            return result;
        }
    }
}