using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using StubServer.Http;

namespace StubServer.Tests.Acceptance.Http
{
    internal class MethodTests
    {
        private HttpClient _httpClient;
        private HttpRequestMessage _httpRequestMessage;
        private HttpResponseMessage _httpResponseMessage;
        private IHttpStubServer _httpStubServer;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var baseAddress = new Uri("http://localhost:5050");

            _httpStubServer = new HttpStubServer(baseAddress);

            _httpClient = new HttpClient
            {
                BaseAddress = baseAddress
            };
        }

        [Test]
        public void Should_get_correct_response_from_GET_request()
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Get &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_get_correct_response_from_POST_request()
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Post &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.Created));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void Should_get_correct_response_from_PUT_request()
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Put &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.NoContent));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void Should_get_correct_response_from_DELETE_request()
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Delete &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.Unauthorized));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public void Should_get_correct_response_from_HEAD_request()
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Head &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.NotModified));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Head, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotModified));
        }

        [Test]
        public void Should_get_correct_response_from_OPTIONS_request()
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Options &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Options, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
        }

        [Test]
        public void Should_get_correct_response_from_TRACE_request()
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Trace &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.NotFound));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Trace, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _httpRequestMessage.Dispose();
            _httpResponseMessage.Dispose();
            _httpClient.Dispose();
            _httpStubServer.Dispose();
        }
    }
}