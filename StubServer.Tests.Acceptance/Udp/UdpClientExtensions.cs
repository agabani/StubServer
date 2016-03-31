using System.Net;
using System.Net.Sockets;

namespace StubServer.Tests.Acceptance.Udp
{
    internal static class UdpClientExtensions
    {
        internal static int Send(this UdpClient udpClient, byte[] bytes)
        {
            return udpClient.Send(bytes, bytes.Length);
        }

        internal static byte[] Receive(this UdpClient udpClient)
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            return udpClient.Receive(ref ipEndPoint);
        }
    }
}