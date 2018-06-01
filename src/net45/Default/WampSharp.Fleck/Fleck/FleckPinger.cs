using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fleck;
using WampSharp.V2.Transports;

namespace WampSharp.Fleck
{
    internal class FleckPinger : IPinger
    {
        private readonly IWebSocketConnection mConnection;

        public FleckPinger(IWebSocketConnection connection)
        {
            mConnection = connection;
        }

        public Task SendPing(byte[] message)
        {
            return mConnection.SendPing(message);
        }

        public event Action<IList<byte>> OnPong
        {
            add => mConnection.OnPong += new Action<byte[]>(value);
            remove => mConnection.OnPong -= new Action<byte[]>(value);
        }

        public bool IsConnected => mConnection.IsAvailable;

        public void Disconnect()
        {
            mConnection.Close();
        }
    }
}