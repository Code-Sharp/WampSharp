using System;

namespace WampSharp.Core.Listener
{
    public interface IWampConnectionListener<TMessage> : IObservable<IWampConnection<TMessage>>
    {
    }
}