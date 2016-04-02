using System.IO;
using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class NoResponseTests : TcpStubServerTests
    {
        [Test]
        public void Should_not_response_to_non_setup_requests()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            tcpStubServer
                .Setup(message => false)
                .Returns(() => Encoding.UTF8.GetBytes("Not Setup"));

            var tcpClient = NewTcpClient();
            var networkStream = tcpClient.GetStream();

            networkStream.Write(new[] {byte.MinValue});

            // Act
            TestDelegate testDelegate = () => networkStream.Read();

            // Assert
            Assert.Throws<IOException>(testDelegate);

            // Cleanup
            Cleanup(networkStream);
            Cleanup(tcpClient);
            Cleanup(tcpStubServer);
        }
    }
}