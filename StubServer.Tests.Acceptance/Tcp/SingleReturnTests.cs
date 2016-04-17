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
            var tcpStubServer = NewStubServer();

            tcpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Return(() => Encoding.UTF8.GetBytes("John Smith"));

            var tcpClient = NewTcpClient();
            var networkStream = tcpClient.GetStream();

            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));

            // Act
            var bytes = networkStream.Read();

            // Assert
            Assert.That(Encoding.UTF8.GetString(bytes), Is.EqualTo("John Smith"));

            // Cleanup
            Cleanup(networkStream);
            Cleanup(tcpClient);
            Cleanup(tcpStubServer);
        }

        [Test]
        public void Should_return_response_multiple_times()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            tcpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Return(() => Encoding.UTF8.GetBytes("John Smith"));

            var tcpClient = NewTcpClient();
            var networkStream = tcpClient.GetStream();

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John Smith"));

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John Smith"));

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John Smith"));

            // Cleanup
            Cleanup(networkStream);
            Cleanup(tcpClient);
            Cleanup(tcpStubServer);
        }
    }
}