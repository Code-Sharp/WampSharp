using System;

namespace WampSharp.PubSub.Server
{
    public class WampSubscriptionAddEventArgs : WampSubscriptionEventArgs
    {
        private readonly IObserver<object> mObserver;

        public WampSubscriptionAddEventArgs(string sessionId, IObserver<object> observer) : 
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