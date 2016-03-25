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
            var path = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.Method == HttpMethod.Get &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_get_correct_response_from_POST_request()
        {
            var path = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.Method == HttpMethod.Post &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.Created));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void Should_get_correct_response_from_PUT_request()
        {
            var path = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.Method == HttpMethod.Put &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.NoContent));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Put, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void Should_get_correct_response_from_DELETE_request()
        {
            var path = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.Method == HttpMethod.Delete &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.Unauthorized));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public void Should_get_correct_response_from_HEAD_request()
        {
            var path = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.Method == HttpMethod.Head &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.NotModified));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Head, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotModified));
        }

        [Test]
        public void Should_get_correct_response_from_OPTIONS_request()
        {
            var path = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.Method == HttpMethod.Options &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Options, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
        }

        [Test]
        public void Should_get_correct_response_from_TRACE_request()
        {
            var path = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.Method == HttpMethod.Trace &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.NotFound));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Trace, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}