using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;

namespace WampSharp.Core.Dispatch.Handler
{
    /// <summary>
    /// An implementation of <see cref="IWampRequestMapper{TMessage}"/> that
    /// maps requests to methods using <see cref="WampHandlerAttribute"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampRequestMapper<TMessage> : IWampRequestMapper<TMessage>
    {
        #region Members

        private readonly IWampFormatter<TMessage> mFormatter; 
        private readonly IDictionary<WampMessageType, ICollection<WampMethodInfo>> mMapping;
        private readonly WampMethodInfo mMissingMethod;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of <see cref="WampRequestMapper{TMessage}"/>
        /// </summary>
        /// <param name="type">The type to map WAMP requests its methods.</param>
        /// <param name="formatter">A <see cref="IWampFormatter{TMessage}"/> used
        /// to check if a parameter can be binded to given type.</param>
        public WampRequestMapper(Type type, IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
            mMapping = BuildMapping(type);

            mMissingMethod = FindMissingMethod(type);
        }

        private WampMethodInfo FindMissingMethod(Type type)
        {
            Type missingContract =
                type.GetClosedGenericTypeImplementation(typeof (IWampMissingMethodContract<,>));

            if (missingContract == null)
            {
                missingContract =
                    type.GetClosedGenericTypeImplementation(typeof (IWampMissingMethodContract<>));
            }

            if (missingContract != null)
            {
                return new WampMethodInfo(missingContract.GetMethod("Missing"));
            }

            return null;
        }

        private Dictionary<WampMessageType, ICollection<WampMethodInfo>> BuildMapping(Type type)
        {
            IEnumerable<MethodInfo> allMethods =
                type.GetInterfaces().SelectMany
                    (x => x.GetInstanceMethods());

            var relevantMethods =
                allMethods.Select(method => new
                                                {
                                                    Method = method,
                                                    Attribute = method.GetCustomAttribute<WampHandlerAttribute>(true)
                                                })
                          .Where(x => x.Attribute != null);

            var result =
                new Dictionary<WampMessageType, ICollection<WampMethodInfo>>
                    (new WampMessageTypeComparer());

            foreach (var relevantMethod in relevantMethods)
            {
                result.Add(relevantMethod.Attribute.MessageType,
                           new WampMethodInfo(relevantMethod.Method));
            }

            return result;
        }

        #endregion

        #region Implementation

        public WampMethodInfo Map(WampMessage<TMessage> request)
        {
            if (!mMapping.TryGetValue(request.MessageType, out ICollection<WampMethodInfo> candidates))
            {
                return mMissingMethod;
            }
            else
            {
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
        }

        private bool CanBind(WampMethodInfo wampMethodInfo, TMessage[] arguments)
        {
            return wampMethodInfo.Method.GetParameters()
                                 .Where(x => !x.IsDefined(typeof(WampProxyParameterAttribute), true))
                                 .Zip(arguments, (parameterInfo, argument) => new {parameterInfo, argument})
                                 .All(x => mFormatter.CanConvert(x.argument, x.parameterInfo.ParameterType));
        }

        #endregion
    }
}