using System;
using System.Reactive.Subjects;

using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Tests
{
    public class MockControlledWampConnection<TMessage> : IControlledWampConnection<TMessage>
    {
        private readonly Subject<WampMessage<TMessage>> mWampConnection = new Subject<WampMessage<TMessage>>();

        public void OnCompleted()
        {
            mWampConnection.OnCompleted();
        }

        public void OnError(Exception error)
        {
            mWampConnection.OnError(error);
        }

        public void OnNext(WampMessage<TMessage> value)
        {
            mWampConnection.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<WampMessage<TMessage>> observer)
        {
            return mWampConnection.Subscribe(observer);
        }

        public void Connect()
        {
        }
    }
}