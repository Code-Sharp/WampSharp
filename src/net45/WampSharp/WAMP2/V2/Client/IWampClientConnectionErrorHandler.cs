using System;

namespace WampSharp.V2.Client
{
    internal interface IWampClientConnectionErrorHandler
    {
        void OnConnectionError(Exception exception);
        void OnConnectionClosed();
    }
}