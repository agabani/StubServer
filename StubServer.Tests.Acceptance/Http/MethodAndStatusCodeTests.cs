using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using StubServer.Http;

namespace StubServer.Tests.Acceptance.Http
{
    internal class MethodAndStatusCodeTests
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
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotModified)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public void Should_get_correct_response_from_GET_request(HttpStatusCode httpStatusCode)
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Get &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(httpStatusCode));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(httpStatusCode));
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotModified)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public void Should_get_correct_response_from_POST_request(HttpStatusCode httpStatusCode)
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Post &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(httpStatusCode));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(httpStatusCode));
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotModified)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public void Should_get_correct_response_from_PUT_request(HttpStatusCode httpStatusCode)
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Put &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(httpStatusCode));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(httpStatusCode));
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotModified)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public void Should_get_correct_response_from_DELETE_request(HttpStatusCode httpStatusCode)
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Delete &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(httpStatusCode));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(httpStatusCode));
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotModified)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public void Should_get_correct_response_from_HEAD_request(HttpStatusCode httpStatusCode)
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Head &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(httpStatusCode));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Head, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(httpStatusCode));
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotModified)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public void Should_get_correct_response_from_OPTIONS_request(HttpStatusCode httpStatusCode)
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Options &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(httpStatusCode));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Options, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(httpStatusCode));
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotModified)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public void Should_get_correct_response_from_TRACE_request(HttpStatusCode httpStatusCode)
        {
            var path = Guid.NewGuid().ToString();

            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Trace &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(httpStatusCode));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Trace, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(httpStatusCode));
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