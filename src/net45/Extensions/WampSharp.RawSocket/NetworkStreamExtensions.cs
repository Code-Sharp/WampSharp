using System.Net.Sockets;
using System.Threading.Tasks;

namespace WampSharp.RawSocket
{
    public static class NetworkStreamExtensions
    {
        public static Task ReadExactAsync(this NetworkStream networkStream, byte[] buffer)
        {
            return networkStream.ReadExactAsync(buffer, buffer.Length);
        }

        public async static Task ReadExactAsync(this NetworkStream networkStream, byte[] buffer, int length)
        {
            int readBytes = 0;

            while (readBytes != length)
            {
                int currentlyRead = await networkStream.ReadAsync(buffer, readBytes, length - readBytes);
                readBytes += currentlyRead;
            }
        }
    }
}