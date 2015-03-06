using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Logging;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Logs;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;

namespace WampSharp.Core.Proxy
{
    /// <summary>
    /// An implementation of <see cref="IWampOutgoingRequestSerializer{TMessage}"/>.
    /// </summary>
    public class WampOutgoingRequestSerializer<TMessage> : IWampOutgoingRequestSerializer<TMessage>
    {
        private readonly IDictionary<MethodInfo, WampMethodInfo> mMethodToWampMethod =
            new SwapDictionary<MethodInfo, WampMethodInfo>();

        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly ILogger mLogger = WampLoggerFactory.Create(typeof(WampOutgoingRequestSerializer<TMessage>));

        /// <summary>
        /// Initializes a new instance of <see cref="WampOutgoingRequestSerializer{TMessage}"/>.
        /// </summary>
        /// <param name="formatter">The <see cref="IWampFormatter{TMessage}"/> to
        /// serialize arguments with.</param>
        public WampOutgoingRequestSerializer(IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
        }

        public WampMessage<TMessage> SerializeRequest(MethodInfo method, object[] arguments)
        {
            mLogger.DebugFormat("Calling remote peer proxy method: {0}", method);

            WampMethodInfo wampMethod = GetWampMethod(method);

            WampMessageType messageType = wampMethod.MessageType;

            WampMessage<TMessage> result = new WampMessage<TMessage>()
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

            List<TMessage> messageArguments = new List<TMessage>();

            foreach (object argument in argumentsToSerialize)
            {
                TMessage serialized = mFormatter.Serialize(argument);
                messageArguments.Add(serialized);
            }

            if (wampMethod.HasParamsArgument)
            {
                foreach (object argument in paramsArgument)
                {
                    TMessage serialized = mFormatter.Serialize(argument);
                    messageArguments.Add(serialized);
                }                
            }

            result.Arguments = messageArguments.ToArray();

            return result;
        }

        private WampMethodInfo GetWampMethod(MethodInfo method)
        {
            WampMethodInfo result;

            if (mMethodToWampMethod.TryGetValue(method, out result))
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