using System.Reactive.Subjects;

namespace WampSharp.V2
{
    public interface IWampSubject : ISubject<IWampEvent, IWampSerializedEvent>
    {
    }
}