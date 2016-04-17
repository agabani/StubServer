using System;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Smtp
{
    internal class InvalidSetupTests : SmtpStubServerTests
    {
        [Test]
        public void Should_not_allow_invalid_setup_for_functions()
        {
            // Arrange
            var smtpStubServer = NewStubServer();

            // Act
            TestDelegate testDelegate = () => smtpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Then(() => Encoding.UTF8.GetBytes("John Smith"));

            // Assert
            var invalidOperationException = Assert.Throws<InvalidOperationException>(testDelegate);
            Assert.That(invalidOperationException.Message, Is.EqualTo("No Return(s) have been configured."));

            // Cleanup
            Cleanup(smtpStubServer);
        }

        [Test]
        public void Should_not_allow_invalid_setup_for_tasks()
        {
            // Arrange
            var smtpStubServer = NewStubServer();

            // Act
            TestDelegate testDelegate = () => smtpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Then(() => Task.FromResult(Encoding.UTF8.GetBytes("John Smith")));

            // Assert
            var invalidOperationException = Assert.Throws<InvalidOperationException>(testDelegate);
            Assert.That(invalidOperationException.Message, Is.EqualTo("No Return(s) have been configured."));

            // Cleanup
            Cleanup(smtpStubServer);
        }

        [Test]
        public void Should_not_allow_invalid_setup_for_task_with_cancellation_token()
        {
            // Arrange
            var smtpStubServer = NewStubServer();

            // Act
            TestDelegate testDelegate = () => smtpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Then(token => Task.FromResult(Encoding.UTF8.GetBytes("John Smith")));

            // Assert
            var invalidOperationException = Assert.Throws<InvalidOperationException>(testDelegate);
            Assert.That(invalidOperationException.Message, Is.EqualTo("No Return(s) have been configured."));

            // Cleanup
            Cleanup(smtpStubServer);
        }
    }
}