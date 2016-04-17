using System.IO;
using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class ClashingSetupTests : TcpStubServerTests
    {
        [Test]
        public void Should_only_return_first_passing_setup()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            tcpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Return(() => Encoding.UTF8.GetBytes("John Smith"));

            tcpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Return(() => Encoding.UTF8.GetBytes("James Bond"));

            var tcpClient = NewTcpClient();
            var networkStream = tcpClient.GetStream();

            // Act & Assert
            networkStream.Write(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(networkStream.Read()), Is.EqualTo("John Smith"));

            // Act & Assert
            TestDelegate testDelegate = () => networkStream.Read();
            Assert.Throws<IOException>(testDelegate);

            // Cleanup
            Cleanup(networkStream);
            Cleanup(tcpClient);
            Cleanup(tcpStubServer);
        }
    }
}