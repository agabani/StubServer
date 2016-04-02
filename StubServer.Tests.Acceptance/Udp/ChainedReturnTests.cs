using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Udp
{
    internal class ChainedReturnTests : UdpStubServerTests
    {
        [Test]
        public void Should_return_response()
        {
            // Arrange
            var udpStubServer = NewStubServer();

            udpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"))
                .Returns(() => Encoding.UTF8.GetBytes("James Bond"))
                .Returns(() => Encoding.UTF8.GetBytes("Bob Marley"));

            var udpClient = NewUdpClient();

            // Act & Assert
            udpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("John Smith"));

            // Act & Assert
            udpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("James Bond"));

            // Act & Assert
            udpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("Bob Marley"));

            // Cleanup
            Cleanup(udpClient);
            Cleanup(udpStubServer);
        }

        [Test]
        public void Should_return_response_then_repeat_final_response()
        {
            // Arrange
            var udpStubServer = NewStubServer();

            udpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"))
                .Returns(() => Encoding.UTF8.GetBytes("James Bond"));

            var udpClient = NewUdpClient();

            // Act & Assert
            udpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("John Smith"));

            // Act & Assert
            udpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("James Bond"));

            // Act & Assert
            udpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("James Bond"));

            // Act & Assert
            udpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("James Bond"));

            // Cleanup
            Cleanup(udpClient);
            Cleanup(udpStubServer);
        }
    }
}