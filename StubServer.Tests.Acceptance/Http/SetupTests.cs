using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class SetupTests : HttpStubServerTests
    {
        [Test]
        public void Should_return_internal_server_error_if_no_return_has_been_configured_for_a_setup()
        {
            HttpStubServer
                .Setup(message => true);

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }
    }
}