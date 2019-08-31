using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core.Contracts;
using MockRawFormatter = WampSharp.Tests.Wampv2.TestHelpers.MockRawFormatter;

namespace WampSharp.Tests.Wampv2
{
    internal class RequestMapper : IWampRequestMapper<MockRaw>
    {
        private interface IWampAll : IWampClientProxy, IWampServerProxy
        {
        }

        private readonly WampRequestMapper<MockRaw> mRequestMapper =
            new WampRequestMapper<MockRaw>(typeof (IWampAll), new MockRawFormatter());

        public WampMethodInfo Map(WampMessage<MockRaw> request)
        {
            return mRequestMapper.Map(request);
        }
    }
}