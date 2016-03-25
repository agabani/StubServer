using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using StubServer.Http;

namespace StubServer.Tests.Acceptance.Http
{
    internal class StatusCodeTests
    {
        private HttpClient _httpClient;
        private HttpRequestMessage _httpRequestMessage;
        private HttpResponseMessage _httpResponseMessage;
        private IHttpStubServer _httpStubServer;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var baseAddress = new Uri("http://localhost:5051");

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