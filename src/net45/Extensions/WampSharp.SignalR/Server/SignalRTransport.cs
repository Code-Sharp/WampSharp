using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.SignalR
{
    public class SignalRTransport : IWampTransport
    {
        private readonly string mUrl;
        private ISignalRListener mListener;

        public SignalRTransport(string url)
        {
            mUrl = url;
        }

        public IWampConnectionListener<TMessage> GetListener<TMessage>(IWampTextBinding<TMessage> binding)
        {
            if (mListener != null)
            {
                throw new ArgumentException();
            }
            else
            {
                SignalRConnectionListener<TMessage> result = new SignalRConnectionListener<TMessage>(mUrl, binding);
                mListener = result;
                return result;                
            }
        }

        public void Dispose()
        {
            mListener.Dispose();
        }

        public void Open()
        {
            mListener.Open();
        }

        public IWampConnectionListener<TMessage> GetListener<TMessage>(IWampBinding<TMessage> binding)
        {
            IWampTextBinding<TMessage> textBinding = binding as IWampTextBinding<TMessage>;

            if (textBinding == null)
            {
                throw new ArgumentException();
            }
            else
            {
                return GetListener(textBinding);
            }
        }
    }
}