using System.Linq;
using System.Net.Http;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Http
{
    internal class ContractTests : HttpStubServerTests
    {
        [Test]
        public void Should_return_correct_setup()
        {
            // Arrange
            var httpStubServer = NewStubServer();

            // Act
            var returnType = httpStubServer.GetType().GetMethod(nameof(httpStubServer.When)).ReturnType;

            // Assert
            Assert.That(returnType.Name, Does.Contain(nameof(SingleReturn<HttpRequestMessage, HttpResponseMessage>)));
            Assert.That(returnType.GenericTypeArguments.Length, Is.EqualTo(2));
            Assert.That(returnType.GenericTypeArguments.First().Name, Is.EqualTo(nameof(HttpRequestMessage)));
            Assert.That(returnType.GenericTypeArguments.Last().Name, Is.EqualTo(nameof(HttpResponseMessage)));

            // Cleanup
            Cleanup(httpStubServer);
        }
    }
}