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
            // Arrange
            var token = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            httpStubServer
                .Setup(message => message.Headers.Authorization.Scheme == "Basic" &&
                                  message.Headers.Authorization.Parameter == token)
                .Returns(() => new HttpResponseMessage(HttpStatusCode.Forbidden));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Post, "/")
                {
                    Headers = {Authorization = new AuthenticationHeaderValue("Basic", token)}
                })
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }
    }
}