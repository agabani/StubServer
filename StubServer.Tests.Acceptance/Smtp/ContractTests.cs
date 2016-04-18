using System.Linq;
using NUnit.Framework;
using StubServer.Framework;

namespace StubServer.Tests.Acceptance.Smtp
{
    internal class ContractTests : SmtpStubServerTests
    {
        [Test]
        public void Should_return_correct_setup()
        {
            // Arrange
            var smtpStubServer = NewStubServer();

            // Act
            var returnType = smtpStubServer.GetType().GetMethod(nameof(smtpStubServer.When)).ReturnType;

            // Assert
            Assert.That(returnType.Name, Does.Contain(nameof(MultipleReturn<byte[], byte[]>)));
            Assert.That(returnType.GenericTypeArguments.Length, Is.EqualTo(2));
            Assert.That(returnType.GenericTypeArguments.All(type => type.Name.Equals("Byte[]")));

            // Cleanup
            Cleanup(smtpStubServer);
        }
    }
}