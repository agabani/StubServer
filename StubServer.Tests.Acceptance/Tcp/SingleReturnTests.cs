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

            var request = Encoding.UTF8.GetBytes("Hello, World!");

            NetworkStream.Write(request, 0, request.Length);

            // Act
            var response = new byte[256];
            var bytes = NetworkStream.Read(response, 0, response.Length);
            var message = Encoding.UTF8.GetString(response, 0, bytes);

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

            var request = Encoding.UTF8.GetBytes("Hello, World!");

            // Act and Assert
            for (var i = 0; i < 3; i++)
            {
                Console.WriteLine(i);
                NetworkStream.Write(request, 0, request.Length);
                var response = new byte[256];
                var bytes = NetworkStream.Read(response, 0, response.Length);
                var message = Encoding.UTF8.GetString(response, 0, bytes);
                Assert.That(message, Is.EqualTo("John Smith"));
            }
        }
    }
}