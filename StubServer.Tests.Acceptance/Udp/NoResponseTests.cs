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
            UdpStubServer
                .Setup(message => false)
                .Returns(() => Encoding.UTF8.GetBytes("Not Setup"));

            UdpClient.Send(new byte[] { });

            // Act
            TestDelegate testDelegate = () => UdpClient.Receive();

            // Assert
            Assert.Throws<SocketException>(testDelegate);
        }
    }
}