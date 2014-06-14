using System.Collections.Generic;
using System.Reactive.Subjects;

namespace WampSharp.V2
{
    public interface IWampRealmServiceProvider
    {
        // TODO: Add overloads for all options

        TProxy GetOperationProxy<TProxy>();

        ISubject<TEvent> GetSubject<TEvent>(string topicUri);

        IWampSubject GetSubject(string topicUri);
    }
}