using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class DelayedTests : HttpStubServerTests
    {
        [Test]
        public void Should_return_response_after_a_delayed_period()
        {
            // Arrange
            var httpStubServer = NewStubServer();

            httpStubServer
                .When(message => true)
                .Return(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            var stopwatch = new Stopwatch();

            var httpClient = NewHttpClient();

            // Act
            stopwatch.Start();
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();
            stopwatch.Stop();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(stopwatch.Elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(500)));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }

        [Test]
        public void Should_timeout_client()
        {
            // Arrange
            var httpStubServer = NewStubServer();

            httpStubServer
                .When(message => true)
                .Return(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            var httpClient = NewHttpClient();


            // Act
            TestDelegate testDelegate = () => httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult().Dispose();

            // Assert
            Assert.Throws<TaskCanceledException>(testDelegate);

            // Cleanup
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }
    }
}