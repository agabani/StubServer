using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class ChainedReturnTests : TcpStubServerTests
    {
        [Test]
        public void Should_return_response()
        {
            // Arrange
            TcpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"))
                .Returns(() => Encoding.UTF8.GetBytes("James Bond"))
                .Returns(() => Encoding.UTF8.GetBytes("Bob Marley"));

            // Act and Assert
            NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            var response = Encoding.UTF8.GetString(NetworkStream.Read());
            Assert.That(response, Is.EqualTo("John Smith"));

            NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            response = Encoding.UTF8.GetString(NetworkStream.Read());
            Assert.That(response, Is.EqualTo("James Bond"));

            NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            response = Encoding.UTF8.GetString(NetworkStream.Read());
            Assert.That(response, Is.EqualTo("Bob Marley"));

            NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            response = Encoding.UTF8.GetString(NetworkStream.Read());
            Assert.That(response, Is.EqualTo("Bob Marley"));
        }

        [Test]
        public void Should_return_response_then_repeat_final_response()
        {
            // Arrange
            TcpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"))
                .Returns(() => Encoding.UTF8.GetBytes("James Bond"));

            // Act and Assert
            NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            var response = Encoding.UTF8.GetString(NetworkStream.Read());
            Assert.That(response, Is.EqualTo("John Smith"));

            NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            response = Encoding.UTF8.GetString(NetworkStream.Read());
            Assert.That(response, Is.EqualTo("James Bond"));

            NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            response = Encoding.UTF8.GetString(NetworkStream.Read());
            Assert.That(response, Is.EqualTo("James Bond"));

            NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            response = Encoding.UTF8.GetString(NetworkStream.Read());
            Assert.That(response, Is.EqualTo("James Bond"));
        }

        [Test, Ignore("Issue with multiple clients connecting to single server")]
        public void Should_return_chained_request_over_multiple_clients()
        {
            // Arrange
            TcpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"))
                .Returns(() => Encoding.UTF8.GetBytes("James Bond"))
                .Returns(() => Encoding.UTF8.GetBytes("Bob Marley"));

            using (var tcpClient = new TcpClient
            {
                Client = {ReceiveTimeout = (int) TimeSpan.FromSeconds(1).TotalMilliseconds}
            })
            {
                tcpClient.Connect(new IPEndPoint(IPAddress.Loopback, 5053));
                using (var networkStream = tcpClient.GetStream())
                {
                    // Act and Assert

                    NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
                    var response = Encoding.UTF8.GetString(NetworkStream.Read());
                    Assert.That(response, Is.EqualTo("John Smith"));

                    networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
                    response = Encoding.UTF8.GetString(networkStream.Read());
                    Assert.That(response, Is.EqualTo("James Bond"));

                    NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
                    response = Encoding.UTF8.GetString(NetworkStream.Read());
                    Assert.That(response, Is.EqualTo("Bob Marley"));

                    networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
                    response = Encoding.UTF8.GetString(networkStream.Read());
                    Assert.That(response, Is.EqualTo("Bob Marley"));
                }
            }
        }
    }
}

/*
var tcpClient = new TcpClient
{
    Client = { ReceiveTimeout = (int)TimeSpan.FromSeconds(1).TotalMilliseconds }
};
tcpClient.Connect(new IPEndPoint(IPAddress.Loopback, 5053));
var networkStream = tcpClient.GetStream();

// Act and Assert
NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
var response = Encoding.UTF8.GetString(NetworkStream.Read());
Assert.That(response, Is.EqualTo("John Smith"));

networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
response = Encoding.UTF8.GetString(networkStream.Read());
Assert.That(response, Is.EqualTo("James Bond"));

NetworkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
response = Encoding.UTF8.GetString(NetworkStream.Read());
Assert.That(response, Is.EqualTo("Bob Marley"));

networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
response = Encoding.UTF8.GetString(networkStream.Read());
Assert.That(response, Is.EqualTo("Bob Marley"));

// Cleanup
networkStream.Dispose();
tcpClient.Close();
*/
