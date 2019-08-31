using System;

namespace WampSharp.V2.Client
{
    internal interface IWampPendingRequest
    {
        long RequestId { get; set; }
        void SetException(Exception exception);
    }
}