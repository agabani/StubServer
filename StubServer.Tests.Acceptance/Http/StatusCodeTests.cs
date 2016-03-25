using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class StatusCodeTests : HttpStubServerTests
    {
        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotModified)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public void Should_get_correct_response_from_GET_request(HttpStatusCode httpStatusCode)
        {
            var path = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.Method == HttpMethod.Get &&
                                  message.RequestUri.PathAndQuery.Equals("/" + path))
                .Returns(() => new HttpResponseMessage(httpStatusCode));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/" + path))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(httpStatusCode));
        }
    }
}