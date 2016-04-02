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
            // Arrange
            var httpStubServer = NewStubServer();

            httpStubServer
                .Setup(message => ThrowException())
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            Assert.That(httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult(), Is.Not.Empty);

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }

        [Test]
        public void Should_return_stack_trace_on_return_failure()
        {
            // Arrange
            var httpStubServer = NewStubServer();

            httpStubServer
                .Setup(message => true)
                .Returns(() =>
                {
                    ThrowException();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            var httpClient = NewHttpClient();

            // Act
            var httpResponseMessage = httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"))
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            Assert.That(httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult(), Is.Not.Empty);

            // Cleanup
            Cleanup(httpResponseMessage);
            Cleanup(httpClient);
            Cleanup(httpStubServer);
        }

        private static bool ThrowException()
        {
            throw new NotImplementedException();
        }
    }
}