using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using StubServer.Http;

namespace StubServer.Tests.Acceptance
{
    internal class ConnectionTests
    {
        private HttpClient _httpClient;
        private IHttpStubServer _httpStubServer;
        private HttpRequestMessage _httpRequestMessage;
        private HttpResponseMessage _httpResponseMessage;

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
        public void Should_get_OK_response()
        {
            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Get &&
                                  message.RequestUri.PathAndQuery.Equals("/"))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_get_Bad_Request_response()
        {
            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Get &&
                                  message.RequestUri.PathAndQuery.Equals("/badrequest"))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.BadRequest));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/badrequest"))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Should_get_Not_Modified_response()
        {
            _httpStubServer
                .Setup(message => message.Method == HttpMethod.Post)
                .Returns(() => new HttpResponseMessage(HttpStatusCode.NotModified));

            _httpResponseMessage = _httpClient
                .SendAsync(_httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/"))
                .GetAwaiter().GetResult();

            Assert.That(_httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotModified));
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