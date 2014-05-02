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

            // Yuck
            MockConnection<MockRaw>.DirectedConnection casted = connection as MockConnection<MockRaw>.DirectedConnection;
            casted.RaiseConnectionOpen();
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

            public void Connect()
            {
                mListener.OnNewConnection(mConnection.SideBToSideA);
            }

            public void Dispose()
            {
                mConnection.SideAToSideB.Dispose();
            }

            public void Send(WampMessage<MockRaw> message)
            {
                mConnection.SideAToSideB.Send(message);
            }

            public event EventHandler ConnectionOpen
            {
                add
                {
                    mConnection.SideAToSideB.ConnectionOpen += value;
                }
                remove
                {
                    mConnection.SideAToSideB.ConnectionOpen -= value;                    
                }
            }

            public event EventHandler<WampMessageArrivedEventArgs<MockRaw>> MessageArrived
            {
                add
                {
                    mConnection.SideAToSideB.MessageArrived += value;
                }
                remove
                {
                    mConnection.SideAToSideB.MessageArrived -= value;
                }
            }


            public event EventHandler ConnectionClosed
            {
                add
                {
                    mConnection.SideAToSideB.ConnectionClosed += value;
                }
                remove
                {
                    mConnection.SideAToSideB.ConnectionClosed -= value;
                }                
            }

            public event EventHandler<WampConnectionErrorEventArgs> ConnectionError
            {
                add
                {
                    mConnection.SideAToSideB.ConnectionError += value;
                }
                remove
                {
                    mConnection.SideAToSideB.ConnectionError -= value;
                }
            }

        }
    }
}