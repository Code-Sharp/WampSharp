using System;
using WampSharp.V2.Client;

namespace WampSharp.V2.Core
{
    internal class WampRequestIdMapper<T> : WampIdMapper<T>
        where T : IWampPendingRequest
    {
        public void ConnectionError(Exception exception)
        {
            foreach (var pendingRegistration in this)
            {
                T registration;
                TryRemove(pendingRegistration.RequestId, out registration);

                pendingRegistration.SetException(exception);
            }
        }

        public void ConnectionClosed()
        {
            throw new NotImplementedException();
        }
    }
}