using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Contracts;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

namespace WampSharp.Core.Dispatch.Handler
{
    public class WampRequestMapper<TMessage> : IWampRequestMapper<WampMessage<TMessage>>
    {
        #region Members

        private readonly IWampFormatter<TMessage> mFormatter; 
        private readonly IDictionary<WampMessageType, ICollection<WampMethodInfo>> mMapping;

        #endregion

        #region Constructor

        public WampRequestMapper(Type type, IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
            mMapping = BuildMapping(type);
        }

        private Dictionary<WampMessageType, ICollection<WampMethodInfo>> BuildMapping(Type type)
        {
            IEnumerable<MethodInfo> allMethods =
                type.GetInterfaces().SelectMany
                    (x => x.GetMethods(BindingFlags.Instance |
                                       BindingFlags.Public |
                                       BindingFlags.NonPublic));

            var relevantMethods =
                allMethods.Select(method => new
                                                {
                                                    Method = method,
                                                    Attribute = method.GetCustomAttribute<WampHandlerAttribute>(true)
                                                })
                          .Where(x => x.Attribute != null);

            var result =
                new Dictionary<WampMessageType, ICollection<WampMethodInfo>>();

            foreach (var relevantMethod in relevantMethods)
            {
                result.Add(relevantMethod.Attribute.MessageType,
                           new WampMethodInfo(relevantMethod.Method));
            }

            return result;
        }

        #endregion

        public WampMethodInfo Map(WampMessage<TMessage> request)
        {
            ICollection<WampMethodInfo> candidates = mMapping[request.MessageType];

            if (candidates.Count == 1)
            {
                return candidates.First();
            }
            else
            {
                List<WampMethodInfo> overloads =
                    candidates.Where(x => x.ArgumentsCount == request.Arguments.Length)
                              .ToList();                
                
                if (overloads.Count == 1)
                {
                    return overloads.First();
                }

                return overloads.First(x => CanBind(x, request.Arguments));
            }
        }

        private bool CanBind(WampMethodInfo wampMethodInfo, TMessage[] arguments)
        {
            return wampMethodInfo.Method.GetParameters()
                                 .Where(x => x.ParameterType != typeof (IWampClient))
                                 .Zip(arguments, (parameterInfo, argument) => new {parameterInfo, argument})
                                 .All(x => mFormatter.CanConvert(x.argument, x.parameterInfo.ParameterType));
        }
    }
}