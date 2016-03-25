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
            HttpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/A"))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.Redirect)
                {
                    Headers = { Location = new Uri(HttpClient.BaseAddress, "/B")}
                });

            HttpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/B"))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.Redirect)
                {
                    Headers = { Location = new Uri(HttpClient.BaseAddress, "/C") }
                });

            HttpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/C"))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("redirect successful")
                });

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "A"))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult(), Is.EqualTo("redirect successful"));
        }
    }
}