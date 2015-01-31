using System;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;

namespace WampSharp.RawSocket
{
    internal abstract class ConnectionListener : IDisposable
    {
        public abstract void OnNewConnection(RawSocketSession connection);
        public abstract void Dispose();
    }

    internal abstract class ConnectionListener<TMessage> :
        ConnectionListener,
        IWampConnectionListener<TMessage>
    {
        private readonly Subject<IWampConnection<TMessage>> mSubject =
            new Subject<IWampConnection<TMessage>>();

        protected void OnNewConnection(IWampConnection<TMessage> connection)
        {
            mSubject.OnNext(connection);
        }

        public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
        {
            return mSubject.Subscribe(observer);
        }

        public override void Dispose()
        {
            mSubject.OnCompleted();
            mSubject.Dispose();
        }
    }
}