using System;
using WampSharp.Core.Listener;

namespace WampSharp.V2.Core.Listener
{
    // TODO: Rename this.
    public interface IWampConnectionListenerProvider : IDisposable
    {
        void Open();

        IWampConnectionListener<TMessage> GetTextListener<TMessage>(IWampTextBinding<TMessage> binding);

        IWampConnectionListener<TMessage> GetBinaryListener<TMessage>(IWampBinaryBinding<TMessage> binding);
    }
}