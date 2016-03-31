using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Compatibility;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class DelayedTests : TcpStubServerTests
    {
        [Test]
        public void Should_return_response_after_a_delayed_period()
        {
            // Arrange
            TcpStubServer
                .Setup(bytes => true)
                .Returns(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    return Encoding.UTF8.GetBytes("500ms");
                });

            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            NetworkStream.Write(new[] { byte.MinValue, });
            var read = NetworkStream.Read();
            stopwatch.Stop();

            // Assert
            Assert.That(Encoding.UTF8.GetString(read), Is.EqualTo("500ms"));
            Assert.That(stopwatch.Elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(500)));
        }

        [Test]
        public void Should_timeout_client()
        {
            // Arrange
            TcpStubServer
                .Setup(message => true)
                .Returns(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    return Encoding.UTF8.GetBytes("2s");
                });

            NetworkStream.Write(new[] { byte.MinValue });

            // Act
            TestDelegate testDelegate = () => NetworkStream.Read();

            // Assert
            Assert.Throws<IOException>(testDelegate);
        }
    }
}