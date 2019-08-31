using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.V2.Fluent
{
    internal class ScheduledWampConnection<TMessage> : IControlledWampConnection<TMessage>
    {
        private readonly IControlledWampConnection<TMessage> mConnection;
        private readonly IScheduler mScheduler;
        private readonly IEventPatternSource<WampMessageArrivedEventArgs<TMessage>> mMessageArrived;
        private readonly IEventPatternSource<EventArgs> mConnectionOpen;
        private readonly IEventPatternSource<EventArgs> mConnectionClosed;
        private readonly IEventPatternSource<WampConnectionErrorEventArgs> mConnectionError;

        public ScheduledWampConnection(IControlledWampConnection<TMessage> connection, IScheduler scheduler)
        {
            mConnection = connection;
            mScheduler = scheduler;

            mMessageArrived =
                GetEventHandler<WampMessageArrivedEventArgs<TMessage>>
                (x => mConnection.MessageArrived += x,
                    x => mConnection.MessageArrived -= x);

            mConnectionOpen =
                GetEventHandler
                (x => mConnection.ConnectionOpen += x,
                    x => mConnection.ConnectionOpen -= x);

            mConnectionClosed =
                GetEventHandler
                (x => mConnection.ConnectionClosed += x,
                    x => mConnection.ConnectionClosed -= x);

            mConnectionError =
                GetEventHandler<WampConnectionErrorEventArgs>
                (x => mConnection.ConnectionError += x,
                    x => mConnection.ConnectionError -= x);
        }

        private IEventPatternSource<TEventArgs> GetEventHandler<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler,
            Action<EventHandler<TEventArgs>> removeHandler)
            where TEventArgs : EventArgs
        {
            return Observable.FromEventPattern<TEventArgs>
                (addHandler, removeHandler)
                .Select(x => new EventPattern<TEventArgs>
                    (this, x.EventArgs))
                .ObserveOn(mScheduler)
                .ToEventPattern();
        }

        private IEventPatternSource<EventArgs> GetEventHandler(Action<EventHandler> addHandler,
            Action<EventHandler> removeHandler)
        {
            return Observable.FromEventPattern<EventHandler, EventArgs>
                (x => new EventHandler(x), addHandler, removeHandler)
                .Select(x => new EventPattern<EventArgs>
                    (this, x.EventArgs))
                .ObserveOn(mScheduler)
                .ToEventPattern();
        }

        public void Connect()
        {
            mConnection.Connect();
        }

        public void Dispose()
        {
            mConnection.Dispose();
        }

        public void Send(WampMessage<object> message)
        {
            mConnection.Send(message);
        }

        public event EventHandler ConnectionOpen
        {
            add => mConnectionOpen.OnNext += new EventHandler<EventArgs>(value);
            remove => mConnectionOpen.OnNext -= new EventHandler<EventArgs>(value);
        }

        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived
        {
            add => mMessageArrived.OnNext += value;
            remove => mMessageArrived.OnNext -= value;
        }

        public event EventHandler ConnectionClosed
        {
            add => mConnectionClosed.OnNext += new EventHandler<EventArgs>(value);
            remove => mConnectionClosed.OnNext -= new EventHandler<EventArgs>(value);
        }

        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError
        {
            add => mConnectionError.OnNext += value;
            remove => mConnectionError.OnNext -= value;
        }
    }
}