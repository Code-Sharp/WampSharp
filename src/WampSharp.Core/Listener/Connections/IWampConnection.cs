using System.Reactive.Subjects;
using WampSharp.Core.Message;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Represents a WAMP bi-directional connection.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampConnection<TMessage> : ISubject<WampMessage<TMessage>>
    {
    }
}