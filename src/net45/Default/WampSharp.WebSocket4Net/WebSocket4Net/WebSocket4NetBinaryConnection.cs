using System;
using SuperSocket.ClientEngine;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WebSocket4Net;

namespace WampSharp.WebSocket4Net
{
    /// <summary>
    /// Represents a client WebSocket binary connection implemented using WebSocket4Net.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WebSocket4NetBinaryConnection<TMessage> : WebSocket4NetConnection<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        /// <summary>
        /// Creates a new instance of <see cref="WebSocket4NetBinaryConnection{TMessage}"/>
        /// given the underlying <see cref="WebSocket"/> to use and the binary binding to use.
        /// </summary>
        /// <param name="webSocket">The underlying <see cref="WebSocket"/> to use..</param>
        /// <param name="binding">The <see cref="IWampTextBinding{TMessage}"/> to use.</param>
        public WebSocket4NetBinaryConnection(WebSocket webSocket, IWampBinaryBinding<TMessage> binding)
            : base(webSocket, binding)
        {
            mBinding = binding;
            WebSocket.DataReceived += OnDataReceived;
        }

        /// <summary>
        /// Creates a new instance of <see cref="WebSocket4NetBinaryConnection{TMessage}"/>
        /// given the server address to connect to and the binary binding to use.
        /// </summary>
        /// <param name="serverAddress">The server address to connect to.</param>
        /// <param name="binding">The <see cref="IWampBinaryBinding{TMessage}"/> to use.</param>
        public WebSocket4NetBinaryConnection(string serverAddress, IWampBinaryBinding<TMessage> binding)
            : this(serverAddress: serverAddress, binding: binding, configureSecurityOptions: null)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="WebSocket4NetBinaryConnection{TMessage}"/>
        /// given the server address to connect to, the binary binding to use, and an Action 
        /// to configure WebSocket Security Options.
        /// </summary>
        /// <param name="serverAddress">The server address to connect to.</param>
        /// <param name="binding">The <see cref="IWampBinaryBinding{TMessage}"/> to use.</param>
        /// <param name="configureSecurityOptions">If non-null, called to allow custom setup of the WebSocket security options.</param>
        public WebSocket4NetBinaryConnection(string serverAddress, IWampBinaryBinding<TMessage> binding, Action<SecurityOption> configureSecurityOptions)
            : base(serverAddress, binding, configureSecurityOptions)
        {
            mBinding = binding;
            WebSocket.DataReceived += OnDataReceived;
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            WampMessage<TMessage> message = mBinding.Parse(e.Data);

            RaiseMessageArrived(message);
        }

        public override void Send(WampMessage<object> message)
        {
            byte[] bytes = mBinding.Format(message);

            WebSocket.Send(bytes, 0, bytes.Length);
        }

        public override void Dispose()
        {
            WebSocket.DataReceived -= OnDataReceived;
            base.Dispose();
        }
    }
}