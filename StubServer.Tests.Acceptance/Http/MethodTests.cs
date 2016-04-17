using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class MethodTests : HttpStubServerTests
    {
        [Test]
        public void Should_get_correct_response_from_GET_request()
        {
            // Arrange
            var path = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            httpStubServer
                .When(message => message.Method == HttpMethod.Get &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Return(() => new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "/" + path))
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }

        [Test]
        public void Should_get_correct_response_from_POST_request()
        {
            // Arrange
            var path = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            httpStubServer
                .When(message => message.Method == HttpMethod.Post &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Return(() => new HttpResponseMessage(HttpStatusCode.Created));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Post, "/" + path))
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }

        [Test]
        public void Should_get_correct_response_from_PUT_request()
        {
            // Arrange
            var path = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            httpStubServer
                .When(message => message.Method == HttpMethod.Put &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Return(() => new HttpResponseMessage(HttpStatusCode.NoContent));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Put, "/" + path))
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }

        [Test]
        public void Should_get_correct_response_from_DELETE_request()
        {
            // Arrange
            var path = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            httpStubServer
                .When(message => message.Method == HttpMethod.Delete &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Return(() => new HttpResponseMessage(HttpStatusCode.Unauthorized));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Delete, "/" + path))
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }

        [Test]
        public void Should_get_correct_response_from_HEAD_request()
        {
            // Arrange
            var path = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            httpStubServer
                .When(message => message.Method == HttpMethod.Head &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Return(() => new HttpResponseMessage(HttpStatusCode.NotModified));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Head, "/" + path))
                .GetAwaiter().GetResult();

            // Arrange
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotModified));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }

        [Test]
        public void Should_get_correct_response_from_OPTIONS_request()
        {
            // Arrange
            var path = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            httpStubServer
                .When(message => message.Method == HttpMethod.Options &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Return(() => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Options, "/" + path))
                .GetAwaiter().GetResult();

            // Arrange
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }

        [Test]
        public void Should_get_correct_response_from_TRACE_request()
        {
            // Arrange
            var path = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            httpStubServer
                .When(message => message.Method == HttpMethod.Trace &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Return(() => new HttpResponseMessage(HttpStatusCode.NotFound));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Trace, "/" + path))
                .GetAwaiter().GetResult();

            // Arrange
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }
    }
}