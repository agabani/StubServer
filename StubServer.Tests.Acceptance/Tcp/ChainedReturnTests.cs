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
            var tcpStubServer = NewStubServer();

            tcpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"))
                .Returns(() => Encoding.UTF8.GetBytes("James Bond"))
                .Returns(() => Encoding.UTF8.GetBytes("Bob Marley"));

            var tcpClient = NewTcpClient();
            var networkStream = tcpClient.GetStream();

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John Smith"));

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("James Bond"));

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("Bob Marley"));

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("Bob Marley"));

            // Cleanup
            Cleanup(networkStream);
            Cleanup(tcpClient);
            Cleanup(tcpStubServer);
        }

        [Test]
        public void Should_return_response_then_repeat_final_response()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            tcpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John Smith"))
                .Returns(() => Encoding.UTF8.GetBytes("James Bond"));

            var tcpClient = NewTcpClient();
            var networkStream = tcpClient.GetStream();

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John Smith"));

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("James Bond"));

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("James Bond"));

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("James Bond"));

            // Cleanup
            Cleanup(networkStream);
            Cleanup(tcpClient);
            Cleanup(tcpStubServer);
        }
    }
}