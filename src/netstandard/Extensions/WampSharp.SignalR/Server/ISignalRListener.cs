using System;

namespace WampSharp.SignalR
{
    internal interface ISignalRListener : IDisposable
    {
        void Open();
    }
}