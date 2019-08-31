using System.Collections.Generic;
using WampSharp.Core.Serialization;

namespace WampSharp.V2
{
    public interface IWampEventValueTupleConverter<TTuple>
    {
        TTuple ToTuple(IWampSerializedEvent @event);

        TTuple ToTuple<TMessage>(IWampFormatter<TMessage> formatter,
                                 TMessage[] argumentsArray,
                                 IDictionary<string, TMessage> argumentKeywords);

        IWampEvent ToEvent(TTuple tuple);
    }
}