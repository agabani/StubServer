using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class HeaderTests : HttpStubServerTests
    {
        [Test]
        public void Should_read_headers()
        {
            var token = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.Headers.Authorization.Scheme == "Basic" &&
                                  message.Headers.Authorization.Parameter == token)
                .Returns(() => new HttpResponseMessage(HttpStatusCode.Forbidden));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/")
                {
                    Headers = {Authorization = new AuthenticationHeaderValue("Basic", token)}
                })
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}