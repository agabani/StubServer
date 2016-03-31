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
            UdpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"));

            // Act
            UdpClient
                .Send(Encoding.UTF8.GetBytes("Hello, World!"));

            var response = Encoding.UTF8.GetString(UdpClient.Receive());

            // Assert
            Assert.That(response, Is.EqualTo("John Smith"));
        }

        [Test]
        public void Should_return_response_multiple_times()
        {
            // Arrange
            UdpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"));

            // Act and Assert
            for (var i = 0; i < 3; i++)
            {
                UdpClient
                    .Send(Encoding.UTF8.GetBytes("Hello, World!"));

                var response = Encoding.UTF8.GetString(UdpClient.Receive());

                Assert.That(response, Is.EqualTo("John Smith"));
            }
        }
    }
}