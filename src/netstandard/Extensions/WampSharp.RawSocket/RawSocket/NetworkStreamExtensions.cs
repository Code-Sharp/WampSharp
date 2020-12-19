using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IO;

namespace WampSharp.RawSocket
{
    internal static class NetworkStreamExtensions
    {
        public static Task ReadExactAsync(this Stream networkStream, byte[] buffer, int position = 0)
        {
            return ReadExactAsync(networkStream, buffer, position, CancellationToken.None);
        }

        public static Task ReadExactAsync(this Stream networkStream, byte[] buffer, int position, CancellationToken cancellationToken)
        {
            return networkStream.ReadExactAsync(buffer, position, buffer.Length, cancellationToken);
        }

        public static Task ReadExactAsync(this Stream networkStream, byte[] buffer, int position, int length)
        {
            return ReadExactAsync(networkStream, buffer, position, length, CancellationToken.None);
        }

        public static async Task ReadExactAsync(this Stream networkStream, byte[] buffer, int position, int length, CancellationToken cancellationToken)
        {
            int currentPosition = position;
            int readBytes = 0;

            while (readBytes != length)
            {
                int currentlyRead = await networkStream.ReadAsync(buffer, currentPosition, length - readBytes, cancellationToken).ConfigureAwait(false);

                // If we read 0 bytes, we have reached the end of the stream.
                if (currentlyRead == 0)
                {
                    throw new EndOfStreamException();
                }

                readBytes += currentlyRead;
                currentPosition += currentlyRead;
            }
        }

        public static byte[] GetBufferWorkaround(this MemoryStream stream)
        {
            RecyclableMemoryStream memoryStream = stream as RecyclableMemoryStream;

            if (memoryStream != null)
            {
                return memoryStream.GetBuffer();
            }

            return stream.GetBuffer();
        }
    }
}