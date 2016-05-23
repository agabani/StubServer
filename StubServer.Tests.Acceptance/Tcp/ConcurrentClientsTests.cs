using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class ConcurrentClientsTests : TcpStubServerTests
    {
        [Test]
        public void Should_return_chained_request_over_multiple_clients()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            tcpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Return(() => Encoding.UTF8.GetBytes("John Smith"))
                .Return(() => Encoding.UTF8.GetBytes("James Bond"))
                .Return(() => Encoding.UTF8.GetBytes("Bob Marley"));

            var tcpClient1 = NewTcpClient();
            var networkStream1 = tcpClient1.GetStream();

            var tcpClient2 = NewTcpClient();
            var networkStream2 = tcpClient2.GetStream();

            // Act & Assert
            networkStream1.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream1.Read(10)), Is.EqualTo("John Smith"));

            // Act & Assert
            networkStream2.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream2.Read(10)), Is.EqualTo("James Bond"));

            // Act & Assert
            networkStream1.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream1.Read(10)), Is.EqualTo("Bob Marley"));

            // Act & Assert
            networkStream2.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream2.Read(10)), Is.EqualTo("Bob Marley"));

            // Cleanup
            Cleanup(networkStream1);
            Cleanup(tcpClient1);
            Cleanup(networkStream2);
            Cleanup(tcpClient2);
            Cleanup(tcpStubServer);
        }
    }
}