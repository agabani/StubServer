using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class MultipleReturnTests : TcpStubServerTests
    {
        [Test]
        public void Should_return_response()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            tcpStubServer
                .Setup(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Returns(() => Encoding.UTF8.GetBytes("John A Smith"))
                .Then(() => Encoding.UTF8.GetBytes("John B Smith"))
                .Then(() => Encoding.UTF8.GetBytes("John C Smith"));

            var tcpClient = NewTcpClient();
            var networkStream = tcpClient.GetStream();

            // Act
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));

            // Assert
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John A Smith"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John B Smith"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John C Smith"));

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
                .Returns(() => Encoding.UTF8.GetBytes("John A Smith"))
                .Then(() => Encoding.UTF8.GetBytes("John B Smith"))
                .Then(() => Encoding.UTF8.GetBytes("John C Smith"))
                .Returns(() => Encoding.UTF8.GetBytes("James A Bond"))
                .Then(() => Encoding.UTF8.GetBytes("James B Bond"))
                .Then(() => Encoding.UTF8.GetBytes("James C Bond"))
                .Returns(() => Encoding.UTF8.GetBytes("Bob A Marley"))
                .Then(() => Encoding.UTF8.GetBytes("Bob B Marley"))
                .Then(() => Encoding.UTF8.GetBytes("Bob C Marley"));

            var tcpClient = NewTcpClient();
            var networkStream = tcpClient.GetStream();

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John A Smith"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John B Smith"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John C Smith"));

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("James A Bond"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("James B Bond"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("James C Bond"));

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("Bob A Marley"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("Bob B Marley"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("Bob C Marley"));

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("Bob A Marley"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("Bob B Marley"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("Bob C Marley"));

            // Cleanup
            Cleanup(networkStream);
            Cleanup(tcpClient);
            Cleanup(tcpStubServer);
        }
    }
}