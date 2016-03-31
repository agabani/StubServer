using System.IO;
using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class NoRequestTests : TcpStubServerTests
    {
        [Test]
        public void Should_not_response_to_no_request()
        {
            // Arrange
            TcpStubServer
                .Setup(message => true)
                .Returns(() => Encoding.UTF8.GetBytes("Setup"));

            NetworkStream.Write(new byte[] { });

            // Act
            TestDelegate testDelegate = () => NetworkStream.Read();

            // Assert
            Assert.Throws<IOException>(testDelegate);
        }
    }
}