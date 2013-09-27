using System;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Tests.TestHelpers.Integration
{
    public class MockConnectionListener : IWampConnectionListener<MockRaw>
    {
        private readonly ISubject<IWampConnection<MockRaw>> mSubject =
            new ReplaySubject<IWampConnection<MockRaw>>();

        public IDisposable Subscribe(IObserver<IWampConnection<MockRaw>> observer)
        {
            return mSubject.Subscribe(observer);
        }

        public IControlledWampConnection<MockRaw> CreateClientConnection()
        {
            return new ListenerControlledConnection(this);
        }

        private void OnNewConnection(IWampConnection<MockRaw> connection)
        {
            mSubject.OnNext(connection);
        }

        private class ListenerControlledConnection : IControlledWampConnection<MockRaw>
        {
            private readonly MockConnection<MockRaw> mConnection;
            private readonly MockConnectionListener mListener;

            public ListenerControlledConnection(MockConnectionListener listener)
            {
                mConnection = new MockConnection<MockRaw>();
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

            public void OnNext(WampMessage<MockRaw> value)
            {
                mConnection.SideAToSideB.OnNext(value);
            }

            public IDisposable Subscribe(IObserver<WampMessage<MockRaw>> observer)
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