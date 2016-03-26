using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class ExceptionTests : HttpStubServerTests
    {
        [Test]
        public void Should_return_stack_trace_on_setup_failure()
        {
            HttpStubServer
                .Setup(message => ThrowException())
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            Assert.That(HttpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult(), Is.Not.Empty);
        }

        [Test]
        public void Should_return_stack_trace_on_return_failure()
        {
            HttpStubServer
                .Setup(message => true)
                .Returns(() =>
                {
                    ThrowException();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            HttpResponseMessage = HttpClient
                .SendAsync(HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            Assert.That(HttpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            Assert.That(HttpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult(), Is.Not.Empty);
        }

        private static bool ThrowException()
        {
            throw new NotImplementedException();
        }
    }
}