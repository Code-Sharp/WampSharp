using WampSharp.V2.Binding;

namespace WampSharp.RawSocket
{
    internal class TextConnectionListener<TMessage> : ConnectionListener<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public TextConnectionListener(IWampTextBinding<TMessage> binding)
        {
            mBinding = binding;
        }

        public IWampTextBinding<TMessage> Binding
        {
            get
            {
                return mBinding;
            }
        }

        public override void OnNewConnection(RawSession connection)
        {
            OnNewConnection(new RawTextConnection<TMessage>(connection, Binding));
        }
    }
}