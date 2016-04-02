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
            // Arrange
            var randomClientContent = Guid.NewGuid().ToString();
            var randomServerContent = Guid.NewGuid().ToString();

            var httpStubServer = NewStubServer();

            httpStubServer
                .Setup(message => message.Content.ReadAsStringAsync().GetAwaiter().GetResult() == randomClientContent)
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(randomServerContent)
                });

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Post, "/")
                {
                    Content = new StringContent(randomClientContent)
                })
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                Is.EqualTo(randomServerContent));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }
    }
}