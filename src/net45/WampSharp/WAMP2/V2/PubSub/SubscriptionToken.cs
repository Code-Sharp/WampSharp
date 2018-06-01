using System;
using WampSharp.V2.Core;

namespace WampSharp.V2.PubSub
{
    internal class SubscriptionToken : IWampRegistrationSubscriptionToken
    {
        private readonly IDisposable mDisposable;

        public SubscriptionToken(long subscriptionId, IDisposable disposable)
        {
            TokenId = subscriptionId;
            mDisposable = disposable;
        }

        public long TokenId { get; }

        public void Dispose()
        {
            mDisposable.Dispose();
        }
    }
}