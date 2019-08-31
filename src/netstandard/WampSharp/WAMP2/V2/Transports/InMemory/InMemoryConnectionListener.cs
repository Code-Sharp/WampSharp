using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;

namespace WampSharp.V2.Transports
{
    internal class InMemoryConnectionListener<TMessage> : IWampConnectionListener<TMessage>, IDisposable
    {
        private readonly Subject<IWampConnection<TMessage>> mSubject = new Subject<IWampConnection<TMessage>>();
        private readonly IScheduler mServerScheduler;
        private readonly IWampBinding<TMessage> mBinding;

        public InMemoryConnectionListener(IScheduler serverScheduler, IWampBinding<TMessage> binding)
        {
            mServerScheduler = serverScheduler;
            mBinding = binding;
        }

        public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
        {
            return mSubject.Subscribe(observer);
        }

        public IControlledWampConnection<TMessage> CreateClientConnection(IScheduler scheduler)
        {
            Subject<WampMessage<TMessage>> serverInput = 
                new Subject<WampMessage<TMessage>>();
            
            Subject<WampMessage<TMessage>> clientInput = 
                new Subject<WampMessage<TMessage>>();

            Subject<Unit> connectionOpen = new Subject<Unit>();
            Subject<Unit> connectionClosed = new Subject<Unit>();

            InMemoryConnection serverToClient =
                new InMemoryConnection(mBinding, serverInput, clientInput, mServerScheduler, connectionOpen, connectionClosed);

            IWampConnection<TMessage> clientToServer =
                new InMemoryConnection(mBinding, clientInput, serverInput, scheduler, connectionOpen, connectionClosed);

            mSubject.OnNext(clientToServer);

            return serverToClient;
        }

        public void Dispose()
        {
            mSubject.Dispose();
        }

        private class InMemoryConnection : IControlledWampConnection<TMessage>
        {
            private readonly IObservable<WampMessage<TMessage>> mIncoming;
            private readonly IObserver<WampMessage<TMessage>> mOutgoing;
            private readonly IScheduler mScheduler;
            private IDisposable mSubscription;
            private readonly ISubject<Unit> mConnectionOpen;
            private readonly ISubject<Unit> mConnectionClosed;
            private readonly IWampBinding<TMessage> mBinding;

            public InMemoryConnection(IWampBinding<TMessage> binding, IObservable<WampMessage<TMessage>> incoming, IObserver<WampMessage<TMessage>> outgoing, IScheduler scheduler, ISubject<Unit> connectionOpen, ISubject<Unit> connectionClosed)
            {
                mIncoming = incoming;
                mOutgoing = outgoing;
                mConnectionOpen = connectionOpen;
                mScheduler = scheduler;
                mConnectionClosed = connectionClosed;
                mBinding = binding;

                IDisposable connectionClosedSubscription =
                    mConnectionClosed.Take(1)
                                     .Subscribe(x => RaiseConnectionClosed());

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
                mSubscription.Dispose();
                mSubscription = null;
            }

            private void OnNewMessage(WampMessage<TMessage> wampMessage)
            {
                this.MessageArrived?.Invoke(this, new WampMessageArrivedEventArgs<TMessage>(wampMessage));
            }

            public void Connect()
            {
                mConnectionOpen.OnNext(Unit.Default);
            }

            public void Dispose()
            {
                mOutgoing.OnCompleted();
            }

            public void Send(WampMessage<object> message)
            {
                var typedMessage = mBinding.Formatter.SerializeMessage(message);
                mOutgoing.OnNext(typedMessage);
            }

            public event EventHandler ConnectionOpen;
            public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
            public event EventHandler ConnectionClosed;

            protected virtual void RaiseConnectionClosed()
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