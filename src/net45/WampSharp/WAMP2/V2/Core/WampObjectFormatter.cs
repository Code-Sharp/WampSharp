using System;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.Core
{
    internal class WampObjectFormatter : IWampFormatter<object>
    {
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
            MethodInfo genericMethod =
                typeof (WampObjectFormatter).
                    GetMethods(BindingFlags.Instance |
                               BindingFlags.Public)
                                            .First
                    (x => x.Name == "Deserialize" && x.IsGenericMethod);

            // This actually only throws an exception if types don't match.
            object converted =
                genericMethod.MakeGenericMethod(type)
                             .Invoke(this, new object[] {message});

            return converted;
        }

        public object Serialize(object value)
        {
            return value;
        }
    }
}