using System.IO;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WampSharp.V2.Core;

namespace WampSharp.V2.Authentication
{
    internal class WampAuthenticationBinaryBinding<TMessage> : WampAuthenticationBinding<TMessage>,
        IWampBinaryBinding<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        public WampAuthenticationBinaryBinding(IWampBinaryBinding<TMessage> binding,
                                               IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
                                               IWampUriValidator uriValidator)
            : base(binding, sessionAuthenticationFactory, uriValidator)
        {
            mBinding = binding;
        }

        public WampMessage<TMessage> Parse(byte[] raw)
        {
            return mBinding.Parse(raw);
        }

        public byte[] Format(WampMessage<object> message)
        {
            return mBinding.Format(message);
        }

        public byte[] GetBytes(byte[] raw)
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