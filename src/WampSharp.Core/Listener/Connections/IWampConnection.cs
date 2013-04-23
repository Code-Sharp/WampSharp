using System.Reactive.Subjects;
using WampSharp.Core.Message;

namespace WampSharp.Core.Listener
{
    public interface IWampConnection<TMessage> : ISubject<WampMessage<TMessage>>
    {
    }
}