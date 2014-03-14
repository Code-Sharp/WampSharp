using System;
using MsgPack;
using MsgPack.Serialization;
using WampSharp.Core.Serialization;

namespace WampSharp.MsgPack
{
    public class MessagePackFormatter : IWampFormatter<MessagePackObject>
    {
        private readonly SerializationContext mSerializationContext =
            new SerializationContext(){SerializationMethod = SerializationMethod.Map};

        public bool CanConvert(MessagePackObject argument, Type type)
        {
            // Irrelavent
            return true;
        }

        public TTarget Deserialize<TTarget>(MessagePackObject message)
        {
            MessagePackSerializer<TTarget> serializer = 
                mSerializationContext.GetSerializer<TTarget>();
            
            MessagePackSerializer<MessagePackObject> messageSerializer = 
                mSerializationContext.GetSerializer<MessagePackObject>();

            byte[] bytes = 
                messageSerializer.PackSingleObject(message);

            TTarget result = serializer.UnpackSingleObject(bytes);

            return result;
        }

        public object Deserialize(Type type, MessagePackObject message)
        {
            IMessagePackSingleObjectSerializer serializer =
                mSerializationContext.GetSerializer(type);

            MessagePackSerializer<MessagePackObject> messageSerializer =
                mSerializationContext.GetSerializer<MessagePackObject>();

            byte[] bytes =
                messageSerializer.PackSingleObject(message);

            object result = serializer.UnpackSingleObject(bytes);

            return result;
        }

        public MessagePackObject Serialize(object value)
        {
            IMessagePackSingleObjectSerializer serializer =
                mSerializationContext.GetSerializer(value.GetType());

            var bytes =
                serializer.PackSingleObject(value);

            MessagePackSerializer<MessagePackObject> messageSerializer =
                mSerializationContext.GetSerializer<MessagePackObject>();

            MessagePackObject serialized =
                messageSerializer.UnpackSingleObject(bytes);
            
            return serialized;
        }
    }
}