using Newtonsoft.Json;
using WampSharp.Newtonsoft;

namespace WampSharp.Tests.Wampv2.TestHelpers
{
    public class MockRawFormatter : Tests.TestHelpers.MockRawFormatter
    {
        public MockRawFormatter(JsonSerializer serializer) : base(serializer)
        {
            serializer.ContractResolver = new JsonPropertyNameContractResolver();
        }

        public MockRawFormatter() : this(new JsonSerializer())
        {
        }
    }
}