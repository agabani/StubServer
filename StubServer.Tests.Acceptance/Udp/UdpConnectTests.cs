using System.Net;
using System.Net.Sockets;
using System.Text;
using NUnit.Framework;
using StubServer.Udp;

namespace StubServer.Tests.Acceptance.Udp
{
    internal class UdpConnectTests
    {
        [Test]
        public void Should_connect()
        {
            var udpStubServer = new UdpStubServer();

            var udpClient = new UdpClient();
            var ipAddress = IPAddress.Loopback;

            udpClient.Connect(ipAddress, 5051);

            var sendBytes = Encoding.UTF8.GetBytes("Hello, World!");

            udpClient.Send(sendBytes, sendBytes.Length);

            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var receiveBytes = udpClient.Receive(ref ipEndPoint);

            var receive = Encoding.UTF8.GetString(receiveBytes);

            Assert.That(receive, Is.EqualTo("John Smith"));
        }
    }
}