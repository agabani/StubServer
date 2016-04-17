using System;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class InvalidSetupTests : TcpStubServerTests
    {
        [Test]
        public void Should_not_allow_invalid_setup_for_functions()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            // Act
            TestDelegate testDelegate = () => tcpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Then(() => Encoding.UTF8.GetBytes("John Smith"));

            // Assert
            var invalidOperationException = Assert.Throws<InvalidOperationException>(testDelegate);
            Assert.That(invalidOperationException.Message, Is.EqualTo("No Return(s) have been configured."));

            // Cleanup
            Cleanup(tcpStubServer);
        }

        [Test]
        public void Should_not_allow_invalid_setup_for_tasks()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            // Act
            TestDelegate testDelegate = () => tcpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Then(() => Task.FromResult(Encoding.UTF8.GetBytes("John Smith")));

            // Assert
            var invalidOperationException = Assert.Throws<InvalidOperationException>(testDelegate);
            Assert.That(invalidOperationException.Message, Is.EqualTo("No Return(s) have been configured."));

            // Cleanup
            Cleanup(tcpStubServer);
        }

        [Test]
        public void Should_not_allow_invalid_setup_for_task_with_cancellation_token()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            // Act
            TestDelegate testDelegate = () => tcpStubServer
                .When(o => Encoding.UTF8.GetString(o).Equals("Hello, World!"))
                .Then(token => Task.FromResult(Encoding.UTF8.GetBytes("John Smith")));

            // Assert
            var invalidOperationException = Assert.Throws<InvalidOperationException>(testDelegate);
            Assert.That(invalidOperationException.Message, Is.EqualTo("No Return(s) have been configured."));

            // Cleanup
            Cleanup(tcpStubServer);
        }
    }
}