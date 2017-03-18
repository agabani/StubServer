using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Udp
{
    internal class DelayedTests : UdpStubServerTests
    {
        [Test]
        public void Should_return_response_after_a_delayed_period()
        {
            // Arrange
            var udpStubServer = NewStubServer();

            udpStubServer
                .When(bytes => true)
                .Return(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    return Encoding.UTF8.GetBytes("500ms");
                });

            var udpClient = NewUdpClient();

            udpClient.Send(new byte[] {});

            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            var receive = udpClient.Receive();
            stopwatch.Stop();

            // Assert
            Assert.That(Encoding.UTF8.GetString(receive), Is.EqualTo("500ms"));
            Assert.That(stopwatch.Elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(500)));

            // Cleanup
            Cleanup(udpClient);
            Cleanup(udpStubServer);
        }

        [Test]
        public void Should_timeout_client()
        {
            // Arrange
            var udpStubServer = NewStubServer();

            udpStubServer
                .When(message => true)
                .Return(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    return Encoding.UTF8.GetBytes("2s");
                });

            var udpClient = NewUdpClient();

            udpClient.Send(new byte[] {});

            // Act
            TestDelegate testDelegate = () => udpClient.Receive();

            // Assert
            Assert.Throws<SocketException>(testDelegate);

            // Cleanup
            Cleanup(udpClient);
            Cleanup(udpStubServer);
        }
    }
}