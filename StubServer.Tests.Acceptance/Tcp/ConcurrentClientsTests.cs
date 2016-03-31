using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NUnit.Framework;
using StubServer.Tcp;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class ConcurrentClientsTests
    {
        [Test]
        public void Should_return_chained_request_over_multiple_clients()
        {
            // Arrange
            ITcpStubServer tcpStubServer = new TcpStubServer(IPAddress.Loopback, 5053);

            tcpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"))
                .Returns(() => Encoding.UTF8.GetBytes("James Bond"))
                .Returns(() => Encoding.UTF8.GetBytes("Bob Marley"));

            var tcpClient1 = new TcpClient
            {
                Client = {ReceiveTimeout = (int) TimeSpan.FromSeconds(1).TotalMilliseconds}
            };

            tcpClient1.Connect(new IPEndPoint(IPAddress.Loopback, 5053));
            var networkStream1 = tcpClient1.GetStream();

            var tcpClient2 = new TcpClient
            {
                Client = { ReceiveTimeout = (int)TimeSpan.FromSeconds(1).TotalMilliseconds }
            };

            tcpClient2.Connect(new IPEndPoint(IPAddress.Loopback, 5053));
            var networkStream2 = tcpClient2.GetStream();

            // Act and Assert
            networkStream1.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            var response = Encoding.UTF8.GetString(networkStream1.Read());
            Assert.That(response, Is.EqualTo("John Smith"));

            networkStream1.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            response = Encoding.UTF8.GetString(networkStream1.Read());
            Assert.That(response, Is.EqualTo("James Bond"));

            networkStream1.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            response = Encoding.UTF8.GetString(networkStream1.Read());
            Assert.That(response, Is.EqualTo("Bob Marley"));

            networkStream1.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            response = Encoding.UTF8.GetString(networkStream1.Read());
            Assert.That(response, Is.EqualTo("Bob Marley"));

            // Cleanup
            networkStream1.Dispose();
            tcpClient1.Close();

            networkStream2.Dispose();
            tcpClient2.Close();

            tcpStubServer.Dispose();
        }
    }
}