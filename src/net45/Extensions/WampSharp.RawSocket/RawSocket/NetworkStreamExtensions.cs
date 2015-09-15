using System.Net.Sockets;
using System.Threading.Tasks;

namespace WampSharp.RawSocket
{
    internal static class NetworkStreamExtensions
    {
        public static Task ReadExactAsync(this NetworkStream networkStream, byte[] buffer, int position = 0)
        {
            return networkStream.ReadExactAsync(buffer, position, buffer.Length);
        }

        public async static Task ReadExactAsync(this NetworkStream networkStream, byte[] buffer, int position, int length)
        {
            int currentPosition = position;
            int readBytes = 0;

            while (readBytes != length)
            {
                int currentlyRead = await networkStream.ReadAsync(buffer, currentPosition, length - readBytes);
                readBytes += currentlyRead;
                currentPosition += currentlyRead;
            }
        }
    }
}