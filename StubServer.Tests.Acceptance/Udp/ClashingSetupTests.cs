using System.Net.Sockets;
using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Udp
{
    internal class ClashingSetupTests : UdpStubServerTests
    {
        [Test]
        public void Should_only_return_first_passing_setup()
        {
            // Arrange
            var udpStubServer = NewStubServer();

            udpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Return(() => Encoding.UTF8.GetBytes("John Smith"));

            udpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Return(() => Encoding.UTF8.GetBytes("James Bond"));

            var udpClient = NewUdpClient();

            // Act & Assert
            udpClient.Send(Encoding.UTF8.GetBytes("Hello, World!"));
            Assert.That(Encoding.UTF8.GetString(udpClient.Receive()), Is.EqualTo("John Smith"));

            // Act & Assert
            TestDelegate testDelegate = () => udpClient.Receive();
            Assert.Throws<SocketException>(testDelegate);

            // Cleanup
            Cleanup(udpClient);
            Cleanup(udpStubServer);
        }
    }
}