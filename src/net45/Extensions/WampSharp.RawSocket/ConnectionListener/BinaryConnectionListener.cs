using WampSharp.V2.Binding;

namespace WampSharp.RawSocket
{
    internal class BinaryConnectionListener<TMessage> : ConnectionListener<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        public BinaryConnectionListener(IWampBinaryBinding<TMessage> binding)
        {
            mBinding = binding;
        }

        public IWampBinaryBinding<TMessage> Binding
        {
            get
            {
                return mBinding;
            }
        }

        public override void OnNewConnection(RawSocketSession connection)
        {
            OnNewConnection(new RawSocketConnection<TMessage>(connection, Binding));
        }
    }
}