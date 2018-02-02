using System.Reactive.Subjects;

namespace WampSharp.V2
{
    /// <summary>
    /// Represents a <see cref="IAsyncSubject{TSource,TResult}"/> that publishes/receives messages
    /// via a WAMP topic.
    /// </summary>
    public interface IWampAsyncSubject : IAsyncSubject<IWampEvent, IWampSerializedEvent>
    {
    }
}