using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Smtp
{
    internal class ObsoleteTests : SmtpStubServerTests
    {
        [Test]
        public void Setup_should_be_obsolete()
        {
            // Arrange
            var httpStubServer = NewStubServer();

            // Act
            var message = ((ObsoleteAttribute) httpStubServer
                .GetType()
                .GetMethod("Setup")
                .GetCustomAttribute(typeof (ObsoleteAttribute)))
                .Message;

            // Assert
            Assert.That(message, Is.EqualTo("Setup is deprecated, please use When instead."));

            // Cleanup
            Cleanup(httpStubServer);
        }

        [Test]
        public void Returns_should_be_obsolete()
        {
            // Arrange
            var httpStubServer = NewStubServer();

            // Act
            var messages = httpStubServer
                .When(message => true)
                .GetType()
                .GetMethods()
                .Where(info => info.Name == "Returns")
                .Select(info => info.GetCustomAttribute(typeof (ObsoleteAttribute)))
                .Select(attribute => ((ObsoleteAttribute) attribute).Message);

            // Assert
            Assert.That(messages, Has.All.EqualTo("Returns is deprecated, please use Return instead."));

            // Cleanup
            Cleanup(httpStubServer);
        }
    }
}