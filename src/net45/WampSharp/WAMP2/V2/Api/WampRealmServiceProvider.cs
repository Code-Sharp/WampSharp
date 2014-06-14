using System.Reactive.Subjects;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    class WampRealmServiceProvider : IWampRealmServiceProvider
    {
        private IWampRealm mRealm;

        public TProxy GetOperationProxy<TProxy>()
        {
            throw new System.NotImplementedException();
        }

        public ISubject<TEvent> GetSubject<TEvent>(string topicUri)
        {
            throw new System.NotImplementedException();
        }

        public IWampSubject GetSubject(string topicUri)
        {
            return new WampRouterSubject(mRealm.TopicContainer.GetTopicByUri(topicUri));
        }
    }
}