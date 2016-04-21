using System.Linq;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class ContractTests : TcpStubServerTests
    {
        [Test]
        public void Should_return_correct_setup()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            // Act
            var returnType = tcpStubServer.GetType().GetMethod(nameof(tcpStubServer.When)).ReturnType;

            // Assert
            Assert.That(returnType.Name, Does.Contain(nameof(MultipleReturn<byte[], byte[]>)));
            Assert.That(returnType.GenericTypeArguments.Length, Is.EqualTo(2));
            Assert.That(returnType.GenericTypeArguments.All(type => type.Name.Equals("Byte[]")));

            // Cleanup
            Cleanup(tcpStubServer);
        }
    }
}