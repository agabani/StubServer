using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Compatibility;

namespace StubServer.Tests.Acceptance.Udp
{
    internal class DelayedTests : UdpStubServerTests
    {
        [Test]
        public void Should_return_response_after_a_delayed_period()
        {
            // Arrange
            UdpStubServer
                .Setup(bytes => true)
                .Returns(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    return Encoding.UTF8.GetBytes("500ms");
                });

            UdpClient.Send(new byte[] {});

            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            var receive = UdpClient.Receive();
            stopwatch.Stop();

            // Assert
            Assert.That(receive, Is.EqualTo("500ms"));
            Assert.That(stopwatch.Elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(500)));
        }

        [Test]
        public void Should_timeout_client()
        {
            // Arrange
            UdpStubServer
                .Setup(message => true)
                .Returns(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    return Encoding.UTF8.GetBytes("2s");
                });

            UdpClient.Send(new byte[] { });

            // Act
            TestDelegate testDelegate = () => UdpClient.Receive();

            // Assert
            Assert.Throws<SocketException>(testDelegate);
        }
    }
}