using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding.Messages;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Binding.Contracts
{
    public class BatchedControlledWampTextConnection<TMessage> : BatchedWampTextConnection<TMessage>,
        IControlledWampConnection<TMessage>
    {
        private readonly IControlledWampConnection<TMessage> mConnection;

        public BatchedControlledWampTextConnection
            (IControlledWampConnection<TMessage> connection,
                TimeSpan timeSpan,
                int count)
            : base(connection, timeSpan, count)
        {
            mConnection = connection;
        }

        public void Connect()
        {
            mConnection.Connect();
        }
    }

    public class BatchedWampTextConnection<TMessage>
    {
        private readonly Subject<WampMessage<TMessage>> mSubject = new Subject<WampMessage<TMessage>>();
        private readonly IWampConnection<TMessage> mConnection;
        private readonly Task<IList<WampMessage<TMessage>>> mCompletionTask;

        public BatchedWampTextConnection(IWampConnection<TMessage> connection, TimeSpan timeSpan, int count)
        {
            mConnection = connection;

            mConnection.MessageArrived += OnMessageArrived;

            mCompletionTask =
                mSubject.Buffer(timeSpan, count)
                    .Do(SendBulk)
                    .ToTask();
        }

        private void RaiseMessageArrived(WampMessage<TMessage> message)
        {
            var handler = MessageArrived;
            
            if (handler != null)
            {
                var eventArgs = new WampMessageArrivedEventArgs<TMessage>(message);
                handler(this, eventArgs);
            }
        }

        private void OnMessageArrived(object sender, WampMessageArrivedEventArgs<TMessage> e)
        {
            BatchedMessage<TMessage> batchedMessage = e.Message as BatchedMessage<TMessage>;

            foreach (WampMessage<TMessage> message in batchedMessage.Messages)
            {
                RaiseMessageArrived(message);
            }
        }

        private void SendBulk(IList<WampMessage<TMessage>> messages)
        {
            BatchedMessage<TMessage> bulk = 
                new BatchedMessage<TMessage>() {Messages = messages};

            mConnection.Send(bulk);
        }

        public void Dispose()
        {
            mSubject.OnCompleted();
            mCompletionTask.Wait();
            mConnection.Dispose();
        }

        public void Send(WampMessage<TMessage> message)
        {
            mSubject.OnNext(message);
        }

        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError
        {
            add
            {
                mConnection.ConnectionError += value;
            }
            remove
            {
                mConnection.ConnectionError -= value;
            }
        }

        public event EventHandler ConnectionClosed
        {
            add
            {
                mConnection.ConnectionClosed += value;
            }
            remove
            {
                mConnection.ConnectionClosed -= value;
            }
        }

        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;

        public event EventHandler ConnectionOpen
        {
            add
            {
                mConnection.ConnectionOpen += value;
            }
            remove
            {
                mConnection.ConnectionOpen -= value;
            }
        }
    }

    internal class BatchedMessage<TMessage> : WampMessage<TMessage>
    {
        public IList<WampMessage<TMessage>> Messages { get; set; }
    }

    public class BatchedJsonBinding<TMessage> : IWampTextBinding<TMessage>
    {
        private readonly JsonBinding<TMessage> mTextBinding;
        private static string mSeparator = new string(new char[] {(char) 0x18});

        public BatchedJsonBinding(JsonBinding<TMessage> textBinding)
        {
            mTextBinding = textBinding;
        }

        public string Name
        {
            get
            {
                return mTextBinding.Name + ".batched";
            }
        }

        public WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message)
        {
            BatchedMessage<TMessage> casted = message as BatchedMessage<TMessage>;

            if (casted == null)
            {
                return mTextBinding.GetRawMessage(message);
            }
            else
            {
                IEnumerable<string> formatted =
                    casted.Messages.Select(x => mTextBinding.Format(x) + mSeparator);

                string raw =
                    string.Join("", formatted);

                RawMessage<TMessage, string> formattedMessage =
                    new RawMessage<TMessage, string> {Raw = raw};

                return formattedMessage;
            }
        }

        public IWampFormatter<TMessage> Formatter
        {
            get
            {
                return mTextBinding.Formatter;
            }
        }

        public IWampBindingHost CreateHost(IWampHostedRealmContainer realmContainer, IWampConnectionListener<TMessage> connectionListener)
        {
            return mTextBinding.CreateHost(realmContainer, connectionListener);
        }

        public WampMessage<TMessage> Parse(string raw)
        {
            BatchedMessage<TMessage> result = new BatchedMessage<TMessage>();

            string[] splitted =
                raw.Split(new[] {mSeparator}, StringSplitOptions.None);

            var exceptLast = splitted.Take(splitted.Length - 1);

            WampMessage<TMessage>[] parsed =
                exceptLast.Select(x => mTextBinding.Parse(x))
                    .ToArray();

            result.Messages = parsed;

            return result;
        }

        public string Format(WampMessage<TMessage> message)
        {
            WampMessage<TMessage> rawMessage = this.GetRawMessage(message);

            RawMessage<TMessage, string> formated = (RawMessage<TMessage, string>) rawMessage;
            
            return formated.Raw;
        }
    }
}