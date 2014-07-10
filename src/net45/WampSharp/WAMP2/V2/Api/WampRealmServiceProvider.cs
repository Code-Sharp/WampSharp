using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    class WampRealmServiceProvider : IWampRealmServiceProvider
    {
        public Task RegisterCallee(object instance)
        {
            throw new System.NotImplementedException();
        }

        public TProxy GetCalleeProxy<TProxy>()
        {
            throw new System.NotImplementedException();
        }

        public ISubject<TEvent> GetSubject<TEvent>(string topicUri)
        {
            throw new System.NotImplementedException();
        }

        public IWampSubject GetSubject(string topicUri)
        {
            throw new System.NotImplementedException();
        }
    }
}