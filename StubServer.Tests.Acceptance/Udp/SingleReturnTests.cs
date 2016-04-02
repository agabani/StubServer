using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Udp
{
    internal class SingleReturnTests : UdpStubServerTests
    {
        [Test]
        public void Should_return_response()
        {
            // Arrange
            var udpStubServer = NewStubServer();

            udpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"));

            var udpClient = NewUdpClient();

            // Act
            udpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            // Assert
            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("John Smith"));

            // Cleanup
            Cleanup(udpClient);
            Cleanup(udpStubServer);
        }

        [Test]
        public void Should_return_response_multiple_times()
        {
            // Arrange
            var udpStubServer = NewStubServer();

            udpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"));

            var udpClient = NewUdpClient();

            // Act & Assert
            udpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("John Smith"));

            // Act & Assert
            udpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("John Smith"));

            // Act & Assert
            udpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("John Smith"));

            // Cleanup
            Cleanup(udpClient);
            Cleanup(udpStubServer);
        }
    }
}