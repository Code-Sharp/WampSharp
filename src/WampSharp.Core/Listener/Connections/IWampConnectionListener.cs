using System;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Represents a <see cref="IWampConnection{TMessage}"/> listener.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampConnectionListener<TMessage> : IObservable<IWampConnection<TMessage>>
    {
    }
}