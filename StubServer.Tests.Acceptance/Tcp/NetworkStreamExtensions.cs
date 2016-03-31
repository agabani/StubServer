using System.Linq;
using System.Net.Sockets;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal static class NetworkStreamExtensions
    {
        internal static byte[] Read(this NetworkStream networkStream)
        {
            var buffer = new byte[8192];
            var bytes = networkStream.Read(buffer, 0, buffer.Length);
            return buffer.Take(bytes).ToArray();
        }

        internal static void Write(this NetworkStream networkStream, byte[] buffer)
        {
            networkStream.Write(buffer, 0, buffer.Length);
        }
    }
}