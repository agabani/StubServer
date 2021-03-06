﻿using System.IO;
using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class NoRequestTests : TcpStubServerTests
    {
        [Test]
        public void Should_not_response_to_no_request()
        {
            // Arrange
            var tcpStubServer = NewStubServer();

            tcpStubServer
                .When(message => true)
                .Return(() => Encoding.UTF8.GetBytes("Setup"));

            var tcpClient = NewTcpClient();
            var networkStream = tcpClient.GetStream();

            networkStream.Write(new byte[] {});

            // Act
            TestDelegate testDelegate = () => networkStream.Read(5);

            // Assert
            Assert.Throws<IOException>(testDelegate);

            // Cleanup
            Cleanup(networkStream);
            Cleanup(tcpClient);
            Cleanup(tcpStubServer);
        }
    }
}