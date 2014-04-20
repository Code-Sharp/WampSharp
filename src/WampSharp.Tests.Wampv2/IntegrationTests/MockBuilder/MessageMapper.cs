using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.MockBuilder
{
    public class MessageMapper : IMessageMapper
    {
        private readonly IWampRequestMapper<MockRaw> mMapper =
            new WampRequestMapper<MockRaw>(typeof (IWampClient), new MockRawFormatter());

        private readonly JTokenEqualityComparer mComparer = new JTokenEqualityComparer();
        
        public WampMessage<MockRaw> MapRequest(WampMessage<MockRaw> message, IEnumerable<WampMessage<MockRaw>> messages, bool notOnlyArguments)
        {
            WampMethodInfo map = mMapper.Map(message);

            int[] indexes;
            
            if (!notOnlyArguments)
            {
                indexes = Enumerable.Range(0, map.Method.GetParameters().Length)
                                    .ToArray();
            }
            else
            {
                indexes =
                    map.Method.GetParameters().Select((x, i) => new {parameter = x, index = i})
                       .Where(x => !(x.parameter.Name.EndsWith("Id")
                           // || x.parameter.Name == "options" ||
                           // x.parameter.Name == "details"
                           ))
                       .Select(x => x.index)
                       .ToArray();
            }

            JArray formattedIncoming = Format(message, indexes);

            WampMessage<MockRaw> request =
                messages.Where(x => x.MessageType == message.MessageType)
                        .FirstOrDefault(x => AreEquivalent(formattedIncoming, indexes, x, message));

            return request;
        }

        private bool AreEquivalent(JArray formattedIncoming,
                                   int[] indexes,
                                   WampMessage<MockRaw> record,
                                   WampMessage<MockRaw> incoming)
        {
            if (record.Arguments.Length != incoming.Arguments.Length)
            {
                return false;
            }
            else
            {
                JArray formattedRecord = Format(record, indexes);
                return mComparer.Equals(formattedRecord, formattedIncoming);
            }
        }

        private JArray Format(WampMessage<MockRaw> message, int[] indexes)
        {
            JArray array =
                new JArray(indexes.Select(x => message.Arguments[x])
                                  .Select(x => Convert(x)));

            return array;
        }

        private static JToken Convert(MockRaw raw)
        {
            MockRaw[] array = raw.Value as MockRaw[];

            if (array != null)
            {
                return new JArray(array.Select(x => Convert(x)));
            }
            else if (raw.Value == null)
            {
                return new JValue((object)null);
            }
            else
            {
                return JToken.FromObject(raw.Value);
            }
        }
    }
}