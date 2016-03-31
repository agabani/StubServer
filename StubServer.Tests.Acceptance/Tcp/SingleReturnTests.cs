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
            TcpStubServer
                .Setup(b => Encoding.UTF8.GetString(b).Equals("Hello!"))
                .Returns(() => Encoding.UTF8.GetBytes("Hi!"));

            var request = Encoding.UTF8.GetBytes("Hello!");

            NetworkStream.Write(request, 0, request.Length);

            // Act
            var response = new byte[256];
            var bytes = NetworkStream.Read(response, 0, response.Length);

            // Assert
            Assert.That(Encoding.UTF8.GetString(response, 0, bytes), Is.EqualTo("Hi!"));

            // Cleanup
            NetworkStream.Close();
        }
    }
}