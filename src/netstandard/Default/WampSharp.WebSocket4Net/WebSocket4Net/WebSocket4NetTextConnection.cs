using System;
using SuperSocket.ClientEngine;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WebSocket4Net;

namespace WampSharp.WebSocket4Net
{
    /// <summary>
    /// Represents a client WebSocket text connection implemented using WebSocket4Net.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WebSocket4NetTextConnection<TMessage> : WebSocket4NetConnection<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        /// <summary>
        /// Creates a new instance of <see cref="WebSocket4NetTextConnection{TMessage}"/>
        /// given the underlying <see cref="WebSocket"/> to use and the text binding to use.
        /// </summary>
        /// <param name="webSocket">The underlying <see cref="WebSocket"/> to use..</param>
        /// <param name="binding">The <see cref="IWampTextBinding{TMessage}"/> to use.</param>
        public WebSocket4NetTextConnection(WebSocket webSocket, IWampTextBinding<TMessage> binding)
            : base(webSocket, binding)
        {
            mBinding = binding;
            WebSocket.MessageReceived += OnMessageReceived;
        }

        /// <summary>
        /// Creates a new instance of <see cref="WebSocket4NetTextConnection{TMessage}"/>
        /// given the server address to connect to and the text binding to use.
        /// </summary>
        /// <param name="serverAddress">The server address to connect to.</param>
        /// <param name="binding">The <see cref="IWampTextBinding{TMessage}"/> to use.</param>
        public WebSocket4NetTextConnection(string serverAddress, IWampTextBinding<TMessage> binding)
            : this(serverAddress: serverAddress, binding: binding, configureSecurityOptions: null)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="WebSocket4NetTextConnection{TMessage}"/>
        /// given the server address to connect to, the text binding to use, and an Action
        /// to configure WebSocket Security Options.
        /// </summary>
        /// <param name="serverAddress">The server address to connect to.</param>
        /// <param name="binding">The <see cref="IWampTextBinding{TMessage}"/> to use.</param>
        /// <param name="configureSecurityOptions">If non-null, called to allow custom setup of the WebSocket security options.</param>
        public WebSocket4NetTextConnection(string serverAddress, IWampTextBinding<TMessage> binding, Action<SecurityOption> configureSecurityOptions)
            : base(serverAddress, binding, configureSecurityOptions)
        {
            mBinding = binding;
            WebSocket.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            WampMessage<TMessage> message = mBinding.Parse(e.Message);

            RaiseMessageArrived(message);
        }

        public override void Send(WampMessage<object> message)
        {
            string text = mBinding.Format(message);

            WebSocket.Send(text);
        }

        public override void Dispose()
        {
            WebSocket.MessageReceived -= OnMessageReceived;
            base.Dispose();
        }
    }
}