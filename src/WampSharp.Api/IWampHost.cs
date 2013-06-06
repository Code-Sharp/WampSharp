using System;

namespace WampSharp.Api
{
    public interface IWampHost : IDisposable
    {
        void Open();
        void HostService(object instance);
    }
}