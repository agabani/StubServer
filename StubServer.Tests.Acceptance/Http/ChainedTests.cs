﻿using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class ChainedTests : HttpStubServerTests
    {
        [Test]
        public void Should_return_responses_in_order()
        {
            // Arrange
            var httpStubServer = NewStubServer();

            httpStubServer
                .Setup(message => true)
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.NotModified))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

            var httpClient = NewHttpClient();

            // Act & Assert & Cleanup
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Cleanup(httpResponseMessage);

            // Act & Assert & Cleanup
            httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotModified));

            Cleanup(httpResponseMessage);

            // Act & Assert & Cleanup
            httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));

            Cleanup(httpResponseMessage);

            // Act & Assert & Cleanup
            httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));

            Cleanup(httpResponseMessage);

            // Cleanup
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }
    }
}