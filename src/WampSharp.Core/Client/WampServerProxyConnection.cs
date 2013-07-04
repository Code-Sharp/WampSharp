using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Core.Client
{
    /// <summary>
    /// A <see cref="IWampConnection{TMessage}"/> that remembers WELCOME
    /// messages.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampServerProxyConnection<TMessage> : IWampConnection<TMessage>
    {
        private readonly IWampConnection<TMessage> mConnection;
        private readonly IDisposable mWelcomeDispose;
        private readonly IObservable<WampMessage<TMessage>> mClientStream;

        public WampServerProxyConnection(IWampConnection<TMessage> connection)
        {
            mConnection = connection;

            IConnectableObservable<WampMessage<TMessage>> welcomeMessages =
                mConnection
                    .Where(x => x.MessageType == WampMessageType.v1Welcome)
                    .Replay(1);

            IObservable<WampMessage<TMessage>> otherMessages =
                mConnection
                    .Where(x => x.MessageType != WampMessageType.v1Welcome);

            mWelcomeDispose = welcomeMessages.Connect();

            mClientStream = welcomeMessages.Merge(otherMessages);
        }

        public void OnNext(WampMessage<TMessage> value)
        {
            mConnection.OnNext(value);
        }

        public void OnError(Exception error)
        {
            mConnection.OnError(error);
        }

        public void OnCompleted()
        {
            mWelcomeDispose.Dispose();
            mConnection.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<WampMessage<TMessage>> observer)
        {
            return mClientStream.Subscribe(observer);
        }
    }
}