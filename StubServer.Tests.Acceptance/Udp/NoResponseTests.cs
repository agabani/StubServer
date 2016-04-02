using System.Net.Sockets;
using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Udp
{
    internal class NoResponseTests : UdpStubServerTests
    {
        [Test]
        public void Should_not_response_to_non_setup_requests()
        {
            // Arrange
            var udpStubServer = NewStubServer();

            udpStubServer
                .Setup(message => false)
                .Returns(() => Encoding.UTF8.GetBytes("Not Setup"));

            var udpClient = NewUdpClient();

            udpClient.Send(new byte[] {});

            // Act
            TestDelegate testDelegate = () => udpClient.Receive();

            // Assert
            Assert.Throws<SocketException>(testDelegate);

            // Cleanup
            Cleanup(udpClient);
            Cleanup(udpStubServer);
        }
    }
}