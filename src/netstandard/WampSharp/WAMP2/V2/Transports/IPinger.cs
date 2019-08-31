using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WampSharp.V2.Transports
{
    public interface IPinger
    {
        Task SendPing(byte[] message);

        event Action<IList<byte>> OnPong;

        bool IsConnected { get; }

        void Disconnect();
    }
}