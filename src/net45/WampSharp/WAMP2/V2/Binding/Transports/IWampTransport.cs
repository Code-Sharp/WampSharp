using System;
using WampSharp.Core.Listener;

namespace WampSharp.V2.Binding.Transports
{
    public interface IWampTransport : IDisposable
    {
        void Open();

        IWampConnectionListener<TMessage> GetListener<TMessage>(IWampBinding<TMessage> binding);         
    }

    public interface IWampTransport<TRaw> : IWampTransport
    {
        IWampConnectionListener<TMessage> GetListener<TMessage>(IWampTransportBinding<TMessage, TRaw> binding);
    }
}