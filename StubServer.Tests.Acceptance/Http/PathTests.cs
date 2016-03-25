using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class PathTests : HttpStubServerTests
    {
        [Test]
        public void Should_return_configured_response_if_has_been_setup_and_triggered()
        {
            var random = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/respected/path/" + random))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "respected/path/" + random))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_return_Not_Found_response_if_has_not_been_setup()
        {
            var random = Guid.NewGuid().ToString();

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "respected/path/" + random))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Should_return_Not_Found_response_if_has_been_setup_and_not_triggered()
        {
            var random = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/respected/path/" + random))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "unrespected/path/" + random))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}