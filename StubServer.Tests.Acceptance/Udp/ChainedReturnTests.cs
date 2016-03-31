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
            UdpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"))
                .Returns(() => Encoding.UTF8.GetBytes("James Bond"))
                .Returns(() => Encoding.UTF8.GetBytes("Bob Marley"));

            // Act and Assert
            UdpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            var response = Encoding.UTF8.GetString(UdpClient.Receive());

            Assert.That(response, Is.EqualTo("John Smith"));

            UdpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            response = Encoding.UTF8.GetString(UdpClient.Receive());

            Assert.That(response, Is.EqualTo("James Bond"));

            UdpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            response = Encoding.UTF8.GetString(UdpClient.Receive());

            Assert.That(response, Is.EqualTo("Bob Marley"));
        }

        [Test]
        public void Should_return_response_then_repeat_final_response()
        {
            // Arrange
            UdpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"))
                .Returns(() => Encoding.UTF8.GetBytes("James Bond"));

            // Act and Assert
            UdpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            var response = Encoding.UTF8.GetString(UdpClient.Receive());

            Assert.That(response, Is.EqualTo("John Smith"));

            UdpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            response = Encoding.UTF8.GetString(UdpClient.Receive());

            Assert.That(response, Is.EqualTo("James Bond"));

            UdpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            response = Encoding.UTF8.GetString(UdpClient.Receive());

            Assert.That(response, Is.EqualTo("James Bond"));

            UdpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            response = Encoding.UTF8.GetString(UdpClient.Receive());

            Assert.That(response, Is.EqualTo("James Bond"));
        }
    }
}