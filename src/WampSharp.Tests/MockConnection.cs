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

        private class DirectedConnection : IWampConnection<TMessage>
        {
            private readonly IObservable<WampMessage<TMessage>> mIncoming;
            private readonly IObserver<WampMessage<TMessage>> mOutgoing;

            public DirectedConnection(IObservable<WampMessage<TMessage>> incoming,
                                      IObserver<WampMessage<TMessage>> outgoing)
            {
                mIncoming = incoming;
                mOutgoing = outgoing;
            }

            public void OnNext(WampMessage<TMessage> value)
            {
                mOutgoing.OnNext(value);
            }

            public void OnError(Exception error)
            {
                mOutgoing.OnError(error);
            }

            public void OnCompleted()
            {
                mOutgoing.OnCompleted();
            }

            public IDisposable Subscribe(IObserver<WampMessage<TMessage>> observer)
            {
                return mIncoming.Subscribe(observer);
            }
        }
    }
}