using System;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

namespace WampSharp.Tests.TestHelpers
{
    public class MockConnection<TMessage>
    {
        private readonly Subject<WampMessage<TMessage>> mSideAToSideB = new Subject<WampMessage<TMessage>>();
        private readonly Subject<WampMessage<TMessage>> mSideBToSideA = new Subject<WampMessage<TMessage>>();
        private readonly DirectedConnection mSideAToSideBConnection;
        private readonly DirectedConnection mSideBToSideAConnection;

        public MockConnection(IWampFormatter<TMessage> formatter)
        {
            mSideAToSideBConnection = new DirectedConnection(mSideBToSideA, mSideAToSideB, formatter);
            mSideBToSideAConnection = new DirectedConnection(mSideAToSideB, mSideBToSideA, formatter);
        }

        public IDirectedControlledWampConnection<TMessage> SideAToSideB => mSideAToSideBConnection;

        public IDirectedControlledWampConnection<TMessage> SideBToSideA => mSideBToSideAConnection;

        public class DirectedConnection : IDirectedControlledWampConnection<TMessage>
        {
            private readonly IObservable<WampMessage<TMessage>> mIncoming;
            private readonly IObserver<WampMessage<TMessage>> mOutgoing;
            private IDisposable mSubscription;
            private readonly IWampFormatter<TMessage> mFormatter;

            public DirectedConnection(IObservable<WampMessage<TMessage>> incoming,
                IObserver<WampMessage<TMessage>> outgoing,
                IWampFormatter<TMessage> formatter)
            {
                mIncoming = incoming;
                mOutgoing = outgoing;
                mFormatter = formatter;

                mSubscription = mIncoming.Subscribe(x => OnNewMessage(x),
                    ex => OnError(ex),
                    () => OnCompleted());
            }

            private void OnError(Exception exception)
            {
                OnConnectionError(new WampConnectionErrorEventArgs(exception));
            }

            private void OnCompleted()
            {
                OnConnectionClosed();
            }

            private void OnNewMessage(WampMessage<TMessage> wampMessage)
            {
                this.MessageArrived?.Invoke(this, new WampMessageArrivedEventArgs<TMessage>(wampMessage));
            }

            public void Connect()
            {
                this.RaiseConnectionOpen();
            }

            public void SendError(Exception exception)
            {
                mOutgoing.OnError(exception);
            }

            public void Dispose()
            {
                if (mSubscription != null)
                {
                    mSubscription.Dispose();
                    mOutgoing.OnCompleted();
                    mSubscription = null;
                    OnConnectionClosed();
                }
            }

            public void Send(WampMessage<object> message)
            {
                WampMessage<TMessage> typedMessage =
                    mFormatter.SerializeMessage(message);

                mOutgoing.OnNext(typedMessage);
            }

            public event EventHandler ConnectionOpen;
            public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
            public event EventHandler ConnectionClosed;

            protected virtual void OnConnectionClosed()
            {
                ConnectionClosed?.Invoke(this, EventArgs.Empty);
            }

            public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

            protected virtual void OnConnectionError(WampConnectionErrorEventArgs e)
            {
                ConnectionError?.Invoke(this, e);
            }

            private void RaiseConnectionOpen()
            {
                ConnectionOpen?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}