using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.SignalR
{
    /// <summary>
    /// Represents a <see cref="IWampTransport"/> implemented using SignalR.
    /// </summary>
    public class SignalRTransport : IWampTransport
    {
        private readonly string mUrl;
        private ISignalRListener mListener;

        /// <summary>
        /// Creates a new instance of <see cref="SignalRTransport"/> given the url
        /// to host the server at.
        /// </summary>
        /// <param name="url">The url to host the server at.</param>
        public SignalRTransport(string url)
        {
            mUrl = url;
        }

        private IWampConnectionListener<TMessage> GetListener<TMessage>(IWampTextBinding<TMessage> binding)
        {
            if (mListener != null)
            {
                throw new ArgumentException("Listener already set.");
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
                throw new ArgumentException("This transport supports only text binding.", "binding");
            }
            else
            {
                return GetListener(textBinding);
            }
        }
    }
}