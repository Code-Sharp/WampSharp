namespace WampSharp.PubSub.Server
{
    public class WampSubscriptionRemoveEventArgs : WampSubscriptionEventArgs
    {
        public WampSubscriptionRemoveEventArgs(string sessionId) : 
            base(sessionId)
        {
        }
    }
}