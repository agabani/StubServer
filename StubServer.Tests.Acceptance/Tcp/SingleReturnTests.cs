using System;
using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class SingleReturnTests : TcpStubServerTests
    {
        [Test]
        public void Should_return_response()
        {
            // Arrange
            TcpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"));

            NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));

            // Act
            var message = Encoding.UTF8.GetString(NetworkStream.Read());

            // Assert
            Assert.That(message, Is.EqualTo("John Smith"));
        }

        [Test]
        public void Should_return_response_multiple_times()
        {
            // Arrange
            TcpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"));

            // Act and Assert
            for (var i = 0; i < 3; i++)
            {
                NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
                var message = Encoding.UTF8.GetString(NetworkStream.Read());
                Assert.That(message, Is.EqualTo("John Smith"));
            }
        }
    }
}