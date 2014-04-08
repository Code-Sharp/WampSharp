using System;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Tests
{
    public class MockConnection<TMessage>
    {
        private readonly Subject<WampMessage<TMessage>> mSideAToSideB = new Subject<WampMessage<TMessage>>();
        private readonly Subject<WampMessage<TMessage>> mSideBToSideA = new Subject<WampMessage<TMessage>>();

        public IWampConnection<TMessage> SideAToSideB
        {
            get
            {
                return new DirectedConnection(mSideBToSideA, mSideAToSideB);
            }
        }

        public IWampConnection<TMessage> SideBToSideA
        {
            get
            {
                return new DirectedConnection(mSideAToSideB, mSideBToSideA);
            }
        }

        public class DirectedConnection : IWampConnection<TMessage>
        {
            private readonly IObservable<WampMessage<TMessage>> mIncoming;
            private readonly IObserver<WampMessage<TMessage>> mOutgoing;
            private IDisposable mSubscription;

            public DirectedConnection(IObservable<WampMessage<TMessage>> incoming,
                                      IObserver<WampMessage<TMessage>> outgoing)
            {
                mIncoming = incoming;
                mOutgoing = outgoing;

                mSubscription = mIncoming.Subscribe(x => OnNewMessage(x));
            }

            private void OnNewMessage(WampMessage<TMessage> wampMessage)
            {
                EventHandler<WampMessageArrivedEventArgs<TMessage>> messageArrived = 
                    this.MessageArrived;

                if (messageArrived != null)
                {
                    messageArrived(this, new WampMessageArrivedEventArgs<TMessage>(wampMessage));
                }
            }

            public void Dispose()
            {
                mSubscription.Dispose();
                mSubscription = null;
            }

            public void Send(WampMessage<TMessage> message)
            {
                mOutgoing.OnNext(message);
            }

            public event EventHandler ConnectionOpen;
            public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
            public event EventHandler ConnectionClosed;
            public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

            public void RaiseConnectionOpen()
            {
                EventHandler connectionOpen = ConnectionOpen;

                if (connectionOpen != null)
                {
                    connectionOpen(this, EventArgs.Empty);
                }
            }
        }
    }
}