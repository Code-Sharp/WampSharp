using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace WampSharp.V2
{
    public interface IWampRealmServiceProvider
    {
        // TODO: Add overloads for all options
        Task RegisterCallee(object instance);

        TProxy GetCalleeProxy<TProxy>() where TProxy : class;

        ISubject<TEvent> GetSubject<TEvent>(string topicUri);

        IWampSubject GetSubject(string topicUri);
    }
}