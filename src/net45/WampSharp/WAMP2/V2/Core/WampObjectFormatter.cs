using System;
using System.Reflection;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;

namespace WampSharp.V2.Core
{
    public class WampObjectFormatter : IWampFormatter<object>
    {
        public static readonly IWampFormatter<object> Value = new WampObjectFormatter();

        private readonly MethodInfo mSerializeMethod =
            Method.Get((WampObjectFormatter x) => x.Deserialize<object>(default(object)))
                  .GetGenericMethodDefinition();

        private WampObjectFormatter()
        {
        }

        public bool CanConvert(object argument, Type type)
        {
            return type.IsInstanceOfType(argument);
        }

        public TTarget Deserialize<TTarget>(object message)
        {
            return (TTarget) message;
        }

        public object Deserialize(Type type, object message)
        {
            if (type.IsInstanceOfType(message))
            {
                return message;
            }
            else
            {
                // This throws an exception if types don't match.
                object converted =
                    mSerializeMethod.MakeGenericMethod(type)
                                    .Invoke(this, new object[] {message});

                return converted;
            }
        }

        public object Serialize(object value)
        {
            return value;
        }
    }
}