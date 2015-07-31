using System;

namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperationRegistrationToken : IDisposable
    {
        long RegistrationId { get; }
    }
}