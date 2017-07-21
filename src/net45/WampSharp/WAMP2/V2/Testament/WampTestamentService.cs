using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Testament
{
    internal class WampTestamentService : IWampTestamentService, IDisposable
    {
        private readonly IWampTopicContainer mTopicContainer;
        private readonly IWampHostedRealm mRealm;

        private readonly object mLock = new object();

        private IImmutableDictionary<long, IImmutableList<Testament>> mSessionIdToTestaments =
            ImmutableDictionary<long, IImmutableList<Testament>>.Empty;

        public WampTestamentService(IWampHostedRealm realm)
        {
            mRealm = realm;
            mTopicContainer = realm.TopicContainer;
            realm.SessionClosed += OnSessionClosed;
        }

        private void OnSessionClosed(object sender, WampSessionCloseEventArgs e)
        {
            long sessionId = e.SessionId;

            IImmutableList<Testament> testaments;

            lock (mLock)
            {
                if (mSessionIdToTestaments.TryGetValue(sessionId, out testaments))
                {
                    mSessionIdToTestaments.Remove(sessionId);
                }
            }

            if (testaments != null)
            {
                foreach (Testament testament in testaments)
                {
                    mTopicContainer.Publish(testament.PublishOptions,
                                            testament.Topic,
                                            testament.Arguments,
                                            testament.ArgumentsKeywords);
                }
            }
        }

        public void AddTestament(string topic,
                                 object[] arguments,
                                 IDictionary<string, object> argumentsKeywords,
                                 PublishOptions publishOptions,
                                 string scope = WampTestamentScope.Destroyed)
        {
            if (!WampTestamentScope.Scopes.Contains(scope))
            {
                throw new WampException("wamp.error.testament_error", "scope must be destroyed or detatched");
            }

            InvocationDetails invocationDetails = WampInvocationContext.Current.InvocationDetails;

            long sessionId = (long) invocationDetails.Caller;

            lock (mLock)
            {
                IImmutableList<Testament> testaments;

                if (!mSessionIdToTestaments.TryGetValue(sessionId, out testaments))
                {
                    testaments = ImmutableList<Testament>.Empty;
                }

                testaments = testaments.Add(new Testament(topic, arguments, argumentsKeywords, publishOptions, scope));

                mSessionIdToTestaments = mSessionIdToTestaments.SetItem(sessionId, testaments);
            }
        }

        public int FlushTestaments(string scope = WampTestamentScope.Destroyed)
        {
            InvocationDetails invocationDetails = WampInvocationContext.Current.InvocationDetails;

            long sessionId = (long)invocationDetails.Caller;

            int result = 0;

            lock (mLock)
            {
                IImmutableList<Testament> testaments;

                if (mSessionIdToTestaments.TryGetValue(sessionId, out testaments))
                {
                    result = testaments.Count;
                }

                mSessionIdToTestaments = mSessionIdToTestaments.Remove(sessionId);

                return result;
            }
        }

        public void Dispose()
        {
            mRealm.SessionClosed -= OnSessionClosed;
        }

        private class Testament
        {
            private static readonly PublishOptions mDefaultPublishOptions = new PublishOptions();

            public Testament(string topic, object[] arguments, IDictionary<string, object> argumentsKeywords, PublishOptions publishOptions, string scope = WampTestamentScope.Destroyed)
            {
                Topic = topic;
                Arguments = arguments;
                ArgumentsKeywords = argumentsKeywords;

                PublishOptions = GetPublishOptions(publishOptions);

                Scope = scope;
            }

            public string Topic { get; private set; }

            public object[] Arguments { get; private set; }

            public IDictionary<string, object> ArgumentsKeywords { get; private set; }

            public PublishOptions PublishOptions { get; private set; }

            public string Scope { get; private set; }

            private static PublishOptions GetPublishOptions(PublishOptions publishOptions)
            {
                PublishOptions result = publishOptions ?? mDefaultPublishOptions;
                result.Acknowledge = null;
                result.DiscloseMe = null;
                result.ExcludeMe = null;
                return result;
            }
        }
    }
}