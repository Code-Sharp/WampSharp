using System.IO;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WampSharp.V2.Core;

namespace WampSharp.V2.Authentication
{
    internal class WampAuthenticationTextBinding<TMessage> : WampAuthenticationBinding<TMessage>,
        IWampTextBinding<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public WampAuthenticationTextBinding(IWampTextBinding<TMessage> binding,
                                             IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
                                             IWampUriValidator uriValidator)
            : base(binding, sessionAuthenticationFactory, uriValidator)
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

        public byte[] GetBytes(string raw)
        {
            return mBinding.GetBytes(raw);
        }

        public WampMessage<TMessage> Parse(Stream stream)
        {
            return mBinding.Parse(stream);
        }

        public void Format(WampMessage<object> message, Stream stream)
        {
            mBinding.Format(message, stream);
        }

        public bool? ComputeBytes
        {
            get => mBinding.ComputeBytes;
            set => mBinding.ComputeBytes = value;
        }
    }
}