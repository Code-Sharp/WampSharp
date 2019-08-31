using System;
using System.Collections.Generic;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Core
{
    internal class WampRequestIdMapper<T> : WampIdMapper<T>
        where T : IWampPendingRequest
    {
        public void ConnectionError(Exception exception)
        {
            SetException(exception);
        }

        public void ConnectionClosed(WampSessionCloseEventArgs eventArgs)
        {
            WampConnectionBrokenException exception = 
                new WampConnectionBrokenException(eventArgs);

            SetException(exception);
        }

        private void SetException(Exception exception)
        {
            ICollection<T> pendingRegistrations = mIdToValue.Values;

            foreach (T pendingRegistration in pendingRegistrations)
            {
                TryRemove(pendingRegistration.RequestId, out T registration);

                pendingRegistration.SetException(exception);
            }
        }
    }
}