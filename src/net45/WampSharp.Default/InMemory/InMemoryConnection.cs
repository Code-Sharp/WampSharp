using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.InMemory
{
    public class InMemoryTransport : IWampTransport
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public IWampConnectionListener<TMessage> GetListener<TMessage>(IWampBinding<TMessage> binding)
        {
            throw new NotImplementedException();
        }
    }

    public class InMemoryConnectionListener<TMessage> : IWampConnectionListener<TMessage>
    {
        private readonly Subject<IWampConnection<TMessage>> mSubject = new Subject<IWampConnection<TMessage>>();
        private readonly IScheduler mServerScheduler;

        public InMemoryConnectionListener(IScheduler serverScheduler)
        {
            mServerScheduler = serverScheduler;
        }

        public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
        {
            return mSubject.Subscribe(observer);
        }

        public IWampConnection<TMessage> CreateClientConnection(IScheduler scheduler)
        {
            Subject<WampMessage<TMessage>> serverInput = 
                new Subject<WampMessage<TMessage>>();
            
            Subject<WampMessage<TMessage>> clientInput = 
                new Subject<WampMessage<TMessage>>();

            Subject<Unit> connectionOpen = new Subject<Unit>();
            Subject<Unit> connectionClosed = new Subject<Unit>();

            IWampConnection<TMessage> serverToClient =
                new InMemoryConnection(serverInput, clientInput, mServerScheduler, connectionOpen, connectionClosed);

            IWampConnection<TMessage> clientToServer =
                new InMemoryConnection(clientInput, serverInput, scheduler, connectionOpen, connectionClosed);

            mSubject.OnNext(clientToServer);

            return serverToClient;
        }

        private class InMemoryConnection : IControlledWampConnection<TMessage>
        {
            private readonly IObservable<WampMessage<TMessage>> mIncoming;
            private readonly IObserver<WampMessage<TMessage>> mOutgoing;
            private readonly IScheduler mScheduler;
            private IDisposable mSubscription;
            private readonly ISubject<Unit> mConnectionOpen;
            private readonly ISubject<Unit> mConnectionClosed;

            public InMemoryConnection(IObservable<WampMessage<TMessage>> incoming, IObserver<WampMessage<TMessage>> outgoing, IScheduler scheduler, ISubject<Unit> connectionOpen, ISubject<Unit> connectionClosed)
            {
                mIncoming = incoming;
                mOutgoing = outgoing;
                mConnectionOpen = connectionOpen;
                mScheduler = scheduler;
                mConnectionClosed = connectionClosed;

                IDisposable connectionClosedSubscription =
                    mConnectionOpen.Subscribe(x => RaiseConnectionClosed());

                IDisposable connectionOpenSubscription =
                    mConnectionOpen.Subscribe(x => RaiseConnectionOpen());

                IDisposable inputSubscription = mIncoming
                    .ObserveOn(mScheduler)
                    .Subscribe(x => OnNewMessage(x),
                        ex => OnError(ex),
                        () => OnCompleted());

                mSubscription =
                    new CompositeDisposable(connectionClosedSubscription,
                        connectionOpenSubscription,
                        inputSubscription);
            }

            private void OnError(Exception exception)
            {
                OnConnectionError(new WampConnectionErrorEventArgs(exception));
            }

            private void OnCompleted()
            {
                mConnectionClosed.OnNext(Unit.Default);
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

            public void Connect()
            {
                mConnectionOpen.OnNext(Unit.Default);
            }

            public void Dispose()
            {
                mSubscription.Dispose();
                mOutgoing.OnCompleted();
                mSubscription = null;
            }

            public void Send(WampMessage<TMessage> message)
            {
                mOutgoing.OnNext(message);
            }

            public event EventHandler ConnectionOpen;
            public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
            public event EventHandler ConnectionClosed;

            protected virtual void RaiseConnectionClosed()
            {
                EventHandler handler = ConnectionClosed;
                if (handler != null) handler(this, EventArgs.Empty);
            }

            public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

            protected virtual void OnConnectionError(WampConnectionErrorEventArgs e)
            {
                EventHandler<WampConnectionErrorEventArgs> handler = ConnectionError;
                if (handler != null) handler(this, e);
            }

            private void RaiseConnectionOpen()
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