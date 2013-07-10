namespace WampSharp.PubSub.Server
{
    public class WampSubscriptionRemovedEventArgs : WampSubscriptionEventArgs
    {
        public WampSubscriptionRemovedEventArgs(string sessionId) : 
            base(sessionId)
        {
        }
    }
}