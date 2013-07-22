using System;

namespace WampSharp.PubSub.Server
{
    public class WampSubscriptionAddedEventArgs : WampSubscriptionEventArgs
    {
        private readonly IObserver<object> mObserver;

        public WampSubscriptionAddedEventArgs(string sessionId, IObserver<object> observer) : 
            base(sessionId)
        {
            mObserver = observer;
        }

        public IObserver<object> Observer
        {
            get
            {
                return mObserver;
            }
        }
    }
}