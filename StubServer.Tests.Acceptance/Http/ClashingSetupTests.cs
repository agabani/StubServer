using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class ClashingSetupTests : HttpStubServerTests
    {
        [Test]
        public void Should_only_return_first_passing_setup()
        {
            // Arrange
            var httpStubServer = NewStubServer();

            httpStubServer
                .When(message => true)
                .Return(() => new HttpResponseMessage(HttpStatusCode.OK));

            httpStubServer
                .When(message => true)
                .Return(() => new HttpResponseMessage(HttpStatusCode.Created));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }
    }
}