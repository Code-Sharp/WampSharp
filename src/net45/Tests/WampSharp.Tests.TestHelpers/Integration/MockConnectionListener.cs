using System;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

namespace WampSharp.Tests.TestHelpers.Integration
{
    public class MockConnectionListener<TMessage> : IWampConnectionListener<TMessage>
    {
        private readonly ISubject<IWampConnection<TMessage>> mSubject =
            new ReplaySubject<IWampConnection<TMessage>>();

        private readonly IWampFormatter<TMessage> mFormatter;

        public MockConnectionListener(IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
        }

        public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
        {
            return mSubject.Subscribe(observer);
        }

        public IControlledWampConnection<TMessage> CreateClientConnection()
        {
            return new ListenerControlledConnection(this, mFormatter);
        }

        private void OnNewConnection(IWampConnection<TMessage> connection)
        {
            mSubject.OnNext(connection);

            // Yuck
            IControlledWampConnection<TMessage> casted = connection as IControlledWampConnection<TMessage>;
            casted.Connect();
        }

        private class ListenerControlledConnection : IControlledWampConnection<TMessage>
        {
            private readonly MockConnection<TMessage> mConnection;
            private readonly MockConnectionListener<TMessage> mListener;

            public ListenerControlledConnection(MockConnectionListener<TMessage> listener, IWampFormatter<TMessage> formatter)
            {
                mConnection = new MockConnection<TMessage>(formatter);
                mListener = listener;
            }

            public void Connect()
            {
                mListener.OnNewConnection(mConnection.SideBToSideA);

                mConnection.SideAToSideB.Connect();
            }

            public void Dispose()
            {
                mConnection.SideAToSideB.Dispose();
            }

            public void Send(WampMessage<object> message)
            {
                mConnection.SideAToSideB.Send(message);
            }

            public event EventHandler ConnectionOpen
            {
                add => mConnection.SideAToSideB.ConnectionOpen += value;
                remove => mConnection.SideAToSideB.ConnectionOpen -= value;
            }

            public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived
            {
                add => mConnection.SideAToSideB.MessageArrived += value;
                remove => mConnection.SideAToSideB.MessageArrived -= value;
            }


            public event EventHandler ConnectionClosed
            {
                add => mConnection.SideAToSideB.ConnectionClosed += value;
                remove => mConnection.SideAToSideB.ConnectionClosed -= value;
            }

            public event EventHandler<WampConnectionErrorEventArgs> ConnectionError
            {
                add => mConnection.SideAToSideB.ConnectionError += value;
                remove => mConnection.SideAToSideB.ConnectionError -= value;
            }

        }
    }
}