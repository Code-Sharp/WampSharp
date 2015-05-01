using System;
using WampSharp.Core.Listener;

namespace WampSharp.Tests.TestHelpers
{
    public interface IDirectedControlledWampConnection<TMessage> : 
        IControlledWampConnection<TMessage>
    {
        void SendError(Exception exception);
    }
}