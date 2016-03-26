using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class ChainedTests : HttpStubServerTests
    {
        [Test]
        public void Should_return_responses_in_order()
        {
            HttpStubServer
                .Setup(message => true)
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.NotModified))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotModified));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
        }
    }
}
