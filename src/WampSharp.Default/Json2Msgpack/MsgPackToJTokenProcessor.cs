using System.Collections.Generic;
using MsgPack;
using Newtonsoft.Json.Linq;

namespace Json2Msgpack
{
    internal class MsgPackToJTokenProcessor
    {
        public JToken Process(MessagePackObject message)
        {
            if (message.IsArray)
            {
                return InnerProcess(message.AsList());
            }
            else if (message.IsDictionary)
            {
                return InnerProcess(message.AsDictionary());
            }

            return InnerProcess(message);
        }

        private JToken InnerProcess(MessagePackObject value)
        {
            return new JValue(value.ToObject());
        }

        private JToken InnerProcess(MessagePackObjectDictionary dictionary)
        {
            JObject result = new JObject();

            foreach (KeyValuePair<MessagePackObject, MessagePackObject> keyValuePair in dictionary)
            {
                string key = keyValuePair.Key.ToObject() as string;

                result[key] = Process(keyValuePair.Value);
            }

            return result;
        }

        private JToken InnerProcess(IList<MessagePackObject> array)
        {
            JArray result = new JArray();

            foreach (MessagePackObject current in array)
            {
                result.Add(Process(current));
            }

            return result;
        }
    }
}