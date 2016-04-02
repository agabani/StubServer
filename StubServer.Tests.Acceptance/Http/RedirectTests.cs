using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class RedirectTests : HttpStubServerTests
    {
        [Test]
        public void Should_redirect_client()
        {
            // Arrange
            var httpStubServer = NewStubServer();

            httpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/A"))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.Redirect)
                {
                    Headers = { Location = new Uri(BaseAddress, "/B")}
                });

            httpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/B"))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.Redirect)
                {
                    Headers = { Location = new Uri(BaseAddress, "/C") }
                });

            httpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/C"))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("redirect successful")
                });

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "A"))
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult(), Is.EqualTo("redirect successful"));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }
    }
}