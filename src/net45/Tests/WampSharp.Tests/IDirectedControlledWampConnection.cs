using System;
using WampSharp.Core.Listener;

namespace WampSharp.Tests
{
    public interface IDirectedControlledWampConnection<TMessage> : 
        IControlledWampConnection<TMessage>
    {
        void SendError(Exception exception);
    }
}