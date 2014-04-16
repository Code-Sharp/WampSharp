using System;

namespace WampSharp.V2.Binding
{
    public interface IWampBindingHost : IDisposable
    {
        void Open();
    }
}