using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Compatibility;

namespace StubServer.Tests.Acceptance.Http
{
    internal class DelayedTests : HttpStubServerTests
    {
        [Test]
        public void Should_return_response_after_a_delayed_period()
        {
            HttpStubServer
                .Setup(message => true)
                .Returns(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            var stopwatch = new Stopwatch();

            stopwatch.Start();
            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();
            stopwatch.Stop();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(stopwatch.Elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(500)));
        }

        [Test]
        public void Should_timeout_client()
        {
            HttpStubServer
                .Setup(message => true)
                .Returns(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            TestDelegate testDelegate = () => HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.Throws<TaskCanceledException>(testDelegate);
        }
    }
}