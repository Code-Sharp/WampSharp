using WampSharp.Core.Listener;
using WampSharp.Tests.TestHelpers.Integration;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.Tests.Wampv2.TestHelpers.Integration
{
    internal class MockTransport<TMessage> : IWampTransport
    {
        private readonly MockConnectionListener<TMessage> mListener;

        public MockTransport(MockConnectionListener<TMessage> listener)
        {
            mListener = listener;
        }

        public void Dispose()
        {
        }

        public void Open()
        {
        }

        public IWampConnectionListener<TOther> GetListener<TOther>(IWampBinding<TOther> binding)
        {
            return (IWampConnectionListener<TOther>) mListener;
        }
    }
}