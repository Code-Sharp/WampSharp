using System;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Tests.TestHelpers.Integration
{
    class MockConnectionListener : MockConnectionListener<MockRaw>
    {
         
    }

    public class MockConnectionListener<TMessage> : IWampConnectionListener<TMessage>
    {
        private readonly ISubject<IWampConnection<TMessage>> mSubject =
            new ReplaySubject<IWampConnection<TMessage>>();

        public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
        {
            return mSubject.Subscribe(observer);
        }

        public IControlledWampConnection<TMessage> CreateClientConnection()
        {
            return new ListenerControlledConnection(this);
        }

        private void OnNewConnection(IWampConnection<TMessage> connection)
        {
            mSubject.OnNext(connection);
        }

        private class ListenerControlledConnection : IControlledWampConnection<TMessage>
        {
            private readonly MockConnection<TMessage> mConnection;
            private readonly MockConnectionListener<TMessage> mListener;

            public ListenerControlledConnection(MockConnectionListener<TMessage> listener)
            {
                mConnection = new MockConnection<TMessage>();
                mListener = listener;
            }

            public void OnCompleted()
            {
                mConnection.SideAToSideB.OnCompleted();
            }

            public void OnError(Exception error)
            {
                mConnection.SideAToSideB.OnError(error);
            }

            public void OnNext(WampMessage<TMessage> value)
            {
                mConnection.SideAToSideB.OnNext(value);
            }

            public IDisposable Subscribe(IObserver<WampMessage<TMessage>> observer)
            {
                return mConnection.SideAToSideB.Subscribe(observer);
            }

            public void Connect()
            {
                mListener.OnNewConnection(mConnection.SideBToSideA);
            }
        }
    }
}