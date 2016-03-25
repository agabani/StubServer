using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class ContentTests : HttpStubServerTests
    {
        [Test]
        public void Should_return_content()
        {
            var randomClientContent = Guid.NewGuid().ToString();
            var randomServerContent = Guid.NewGuid().ToString();

            HttpStubServer
                .Setup(message => message.Content.ReadAsStringAsync().GetAwaiter().GetResult() == randomClientContent)
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(randomServerContent)
                });

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/")
                {
                    Content = new StringContent(randomClientContent)
                })
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                Is.EqualTo(randomServerContent));
        }
    }
}