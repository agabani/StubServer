using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class DelayedTests : TcpStubServerTests
    {
        [Test]
        public void Should_return_response_after_a_delayed_period()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            tcpStubServer
                .When(bytes => true)
                .Return(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    return Encoding.UTF8.GetBytes("500ms");
                });

            var tcpClient = NewTcpClient();
            var networkStream = tcpClient.GetStream();

            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            networkStream.Write(new[] {byte.MinValue});
            var read = networkStream.Read(5);
            stopwatch.Stop();

            // Assert
            Assert.That(Encoding.UTF8.GetString(read), Is.EqualTo("500ms"));
            Assert.That(stopwatch.Elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(500)));

            // Cleanup
            Cleanup(networkStream);
            Cleanup(tcpClient);
            Cleanup(tcpStubServer);
        }

        [Test]
        public void Should_timeout_client()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            tcpStubServer
                .When(message => true)
                .Return(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    return Encoding.UTF8.GetBytes("2s");
                });

            var tcpClient = NewTcpClient();
            var networkStream = tcpClient.GetStream();

            networkStream.Write(new[] {byte.MinValue});

            // Act
            TestDelegate testDelegate = () => networkStream.Read(2);

            // Assert
            Assert.Throws<IOException>(testDelegate);

            // Cleanup
            Cleanup(networkStream);
            Cleanup(tcpClient);
            Cleanup(tcpStubServer);
        }
    }
}