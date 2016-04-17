using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Udp
{
    internal class ConcurrentClientsTests : UdpStubServerTests
    {
        [Test]
        public void Should_return_chained_request_over_multiple_clients()
        {
            // Arrange
            var udpStubServer = NewStubServer();

            udpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Return(() => Encoding.UTF8.GetBytes("John Smith"))
                .Return(() => Encoding.UTF8.GetBytes("James Bond"))
                .Return(() => Encoding.UTF8.GetBytes("Bob Marley"));

            var udpClient1 = NewUdpClient();
            var udpClient2 = NewUdpClient();

            // Act & Assert
            udpClient1.Send(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(udpClient1.Receive()), Is.EqualTo("John Smith"));

            // Act & Assert
            udpClient2.Send(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(udpClient2.Receive()), Is.EqualTo("James Bond"));

            // Act & Assert
            udpClient1.Send(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(udpClient1.Receive()), Is.EqualTo("Bob Marley"));

            // Act & Assert
            udpClient2.Send(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(udpClient2.Receive()), Is.EqualTo("Bob Marley"));

            // Cleanup
            Cleanup(udpClient1);
            Cleanup(udpClient2);
            Cleanup(udpStubServer);
        }
    }
}