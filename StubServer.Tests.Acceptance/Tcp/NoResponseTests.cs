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
            TcpStubServer
                .Setup(message => false)
                .Returns(() => Encoding.UTF8.GetBytes("Not Setup"));

            NetworkStream.Write(new[] {byte.MinValue});

            // Act
            TestDelegate testDelegate = () => NetworkStream.Read();

            // Assert
            Assert.Throws<IOException>(testDelegate);
        }
    }
}