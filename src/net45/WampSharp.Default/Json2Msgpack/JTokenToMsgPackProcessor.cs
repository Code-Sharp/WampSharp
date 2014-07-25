using System.Collections.Generic;
using MsgPack;
using MsgPack.Serialization;
using Newtonsoft.Json.Linq;

namespace Json2Msgpack
{
    internal class JTokenToMsgPackProcessor
    {
        private readonly SerializationContext mSerializationContext = 
            new SerializationContext();

        private MessagePackObject InnerProcess(JValue value)
        {
            object objectTree = value.Value;

            IMessagePackSingleObjectSerializer serializer = 
                mSerializationContext.GetSerializer(objectTree.GetType());

            var objectSerializer =
                mSerializationContext.GetSerializer<MessagePackObject>();

            return objectSerializer.UnpackSingleObject
                (serializer.PackSingleObject(objectTree));
        }

        private MessagePackObject InnerProcess(JObject value)
        {
            MessagePackObjectDictionary dictionary = 
                new MessagePackObjectDictionary();

            foreach (KeyValuePair<string, JToken> keyValuePair in value)
            {
                dictionary[keyValuePair.Key] = Process(keyValuePair.Value);
            }

            MessagePackObject result = new MessagePackObject(dictionary);

            return result;
        }

        private MessagePackObject InnerProcess(JArray value)
        {
            List<MessagePackObject> array = 
                new List<MessagePackObject>(value.Count);

            foreach (JToken element in value)
            {
                array.Add(Process(element));
            }

            MessagePackObject result = 
                new MessagePackObject(array);
            
            return result;
        }

        public MessagePackObject Process(JToken value)
        {
            return InnerProcess((dynamic) value);
        }
    }
}