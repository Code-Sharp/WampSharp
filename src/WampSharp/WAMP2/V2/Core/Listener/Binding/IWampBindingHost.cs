using System;

namespace WampSharp.V2.Core.Listener
{
    public interface IWampBindingHost : IDisposable
    {
        void Open();
    }
}