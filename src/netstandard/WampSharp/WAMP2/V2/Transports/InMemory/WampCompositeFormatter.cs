using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;

namespace WampSharp.V2.Transports
{
    internal class WampCompositeFormatter : IWampFormatter<object>
    {
        private readonly IDictionary<Type, CastedFormatter> mFormatters =
            new Dictionary<Type, CastedFormatter>();

        private readonly CastedFormatter<object> mDefaultFormatter;

        public WampCompositeFormatter()
        {
            mDefaultFormatter = new CastedFormatter<object>(WampObjectFormatter.Value);
        }

        private CastedFormatter FindFormatter(Type type)
        {
            KeyValuePair<Type, CastedFormatter> candidate =
                mFormatters.FirstOrDefault(x => x.Key.IsAssignableFrom(type));

            CastedFormatter result = candidate.Value;

            if (result != null)
            {
                return result;
            }
            else
            {
                return mDefaultFormatter;
            }
        }

        public bool CanConvert(object argument, Type type)
        {
            return true;
        }

        public TTarget Deserialize<TTarget>(object message)
        {
            if (message == null)
            {
                return default(TTarget);
            }

            CastedFormatter castedFormatter = FindFormatter(message.GetType());

            return castedFormatter.Deserialize<TTarget>(message);
        }

        public object Deserialize(Type type, object message)
        {
            if (message == null)
            {
                return null;
            }

            CastedFormatter castedFormatter = FindFormatter(message.GetType());

            return castedFormatter.Deserialize(type, message);
        }

        public void AddFormatter<TMessage>(IWampFormatter<TMessage> formatter)
        {
            mFormatters[typeof (TMessage)] = new CastedFormatter<TMessage>(formatter);
        }

        public object Serialize(object value)
        {
            return value;
        }

        #region Nested Types

        private abstract class CastedFormatter
        {
            public abstract TTarget Deserialize<TTarget>(object message);
            public abstract object Deserialize(Type type, object message);
        }

        private class CastedFormatter<TMessage> : CastedFormatter
        {
            private readonly IWampFormatter<TMessage> mFormatter;

            public CastedFormatter(IWampFormatter<TMessage> formatter)
            {
                mFormatter = formatter;
            }

            public override TTarget Deserialize<TTarget>(object message)
            {
                return mFormatter.Deserialize<TTarget>
                    ((TMessage)message);
            }

            public override object Deserialize(Type type, object message)
            {
                return mFormatter.Deserialize
                    (type, (TMessage)message);
            }
        }

        #endregion
    }
}