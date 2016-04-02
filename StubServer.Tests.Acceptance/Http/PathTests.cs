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
            // Arrange
            var random = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            httpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/respected/path/" + random))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "respected/path/" + random))
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }

        [Test]
        public void Should_return_Not_Found_response_if_has_not_been_setup()
        {
            // Arrange
            var random = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "respected/path/" + random))
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }

        [Test]
        public void Should_return_Not_Found_response_if_has_been_setup_and_not_triggered()
        {
            // Arrange
            var random = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            httpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/respected/path/" + random))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "unrespected/path/" + random))
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }
    }
}