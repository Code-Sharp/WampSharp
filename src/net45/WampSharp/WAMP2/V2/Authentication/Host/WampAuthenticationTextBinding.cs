using System.IO;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;

namespace WampSharp.V2.Authentication
{
    internal class WampAuthenticationTextBinding<TMessage> : WampAuthenticationBinding<TMessage>,
        IWampTextBinding<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public WampAuthenticationTextBinding(IWampTextBinding<TMessage> binding,
                                             IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
            : base(binding, sessionAuthenticationFactory)
        {
            mBinding = binding;
        }

        public WampMessage<TMessage> Parse(string raw)
        {
            return mBinding.Parse(raw);
        }

        public string Format(WampMessage<object> message)
        {
            return mBinding.Format(message);
        }

        public WampMessage<TMessage> Parse(Stream stream)
        {
            return mBinding.Parse(stream);
        }

        public void Format(WampMessage<object> message, Stream stream)
        {
            mBinding.Format(message, stream);
        }
    }
}