using System.Linq;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Udp
{
    internal class ContractTests : UdpStubServerTests
    {
        [Test]
        public void Should_return_correct_setup()
        {
            // Arrange
            var udpStubServer = NewStubServer();

            // Act
            var returnType = udpStubServer.GetType().GetMethod(nameof(udpStubServer.When)).ReturnType;

            // Assert
            Assert.That(returnType.Name, Does.Contain(nameof(MultipleReturn<byte[], byte[]>)));
            Assert.That(returnType.GenericTypeArguments.Length, Is.EqualTo(2));
            Assert.That(returnType.GenericTypeArguments.All(type => type.Name.Equals("Byte[]")));

            // Cleanup
            Cleanup(udpStubServer);
        }
    }
}