using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StubServer.Tcp;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal class SampleTests
    {
        [Test]
        public void SimpleTest()
        {
            // Arrange
            var tcpStubServer = new TcpStubServer(IPAddress.Any, 5000);

            tcpStubServer
                .When(bytes => true)
                .Return(() => Encoding.UTF8.GetBytes("Hello, World!"));

            var tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Loopback, 5000);

            var networkStream = tcpClient.GetStream();

            var message = new []{byte.MinValue};

            networkStream.Write(message, 0, message.Length);

            // Act
            var buffer = new byte[8192];
            var read = networkStream.Read(buffer, 0, buffer.Length);

            // Assert
            Assert.That(Encoding.UTF8.GetString(buffer, 0, read), Is.EqualTo("Hello, World!"));

            // Cleanup
            networkStream.Dispose();
            tcpClient.Close();
            tcpStubServer.Dispose();
        }

        [Test]
        public void DemoTest()
        {
            // Arrange
            var tcpStubServer = new TcpStubServer(IPAddress.Any, 5000);

            tcpStubServer
                .When(bytes => Encoding.UTF8.GetString(bytes).Equals("Hi!"))
                .Return(() => Encoding.UTF8.GetBytes("Hello, World!"));

            var tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Loopback, 5000);

            var networkStream = tcpClient.GetStream();

            var message = Encoding.UTF8.GetBytes("Hi!");

            networkStream.Write(message, 0, message.Length);

            // Act
            var buffer = new byte[8192];
            var read = networkStream.Read(buffer, 0, buffer.Length);

            // Assert
            Assert.That(Encoding.UTF8.GetString(buffer, 0, read), Is.EqualTo("Hello, World!"));

            // Cleanup
            networkStream.Dispose();
            tcpClient.Close();
            tcpStubServer.Dispose();
        }

        [Test]
        public void ChainedResponses()
        {
            // Arrange
            var tcpStubServer = new TcpStubServer(IPAddress.Any, 5000);

            tcpStubServer
                .When(bytes => Encoding.UTF8.GetString(bytes).Equals("Hi!"))
                .Return(() => Encoding.UTF8.GetBytes("Hello, John!"))
                .Return(() => Encoding.UTF8.GetBytes("Hello, Tom!"))
                .Return(() => Encoding.UTF8.GetBytes("Hello, Ben!"));

            var tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Loopback, 5000);

            var networkStream = tcpClient.GetStream();

            var message = Encoding.UTF8.GetBytes("Hi!");

            // Act & Assert
            var buffer = new byte[8192];

            networkStream.Write(message, 0, message.Length);
            var read = networkStream.Read(buffer, 0, buffer.Length);
            Assert.That(Encoding.UTF8.GetString(buffer, 0, read), Is.EqualTo("Hello, John!"));

            networkStream.Write(message, 0, message.Length);
            read = networkStream.Read(buffer, 0, buffer.Length);
            Assert.That(Encoding.UTF8.GetString(buffer, 0, read), Is.EqualTo("Hello, Tom!"));

            networkStream.Write(message, 0, message.Length);
            read = networkStream.Read(buffer, 0, buffer.Length);
            Assert.That(Encoding.UTF8.GetString(buffer, 0, read), Is.EqualTo("Hello, Ben!"));

            networkStream.Write(message, 0, message.Length);
            read = networkStream.Read(buffer, 0, buffer.Length);
            Assert.That(Encoding.UTF8.GetString(buffer, 0, read), Is.EqualTo("Hello, Ben!"));
            
            // Cleanup
            networkStream.Dispose();
            tcpClient.Close();
            tcpStubServer.Dispose();
        }

        [Test]
        public void MultipleResponses()
        {
            // Arrange
            var tcpStubServer = new TcpStubServer(IPAddress.Any, 5000);

            tcpStubServer
                .When(bytes => Encoding.UTF8.GetString(bytes).Equals("Hi!"))
                .Return(() => Encoding.UTF8.GetBytes("Hello, John!"))
                .Then(() => Encoding.UTF8.GetBytes("Hello, Tom!"))
                .Then(() => Encoding.UTF8.GetBytes("Hello, Ben!"));

            var tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Loopback, 5000);

            var networkStream = tcpClient.GetStream();

            var message = Encoding.UTF8.GetBytes("Hi!");

            var buffer = new byte[8192];

            // Act
            networkStream.Write(message, 0, message.Length);

            // Assert
            var read = networkStream.Read(buffer, 0, buffer.Length);
            Assert.That(Encoding.UTF8.GetString(buffer, 0, read), Is.EqualTo("Hello, John!"));

            read = networkStream.Read(buffer, 0, buffer.Length);
            Assert.That(Encoding.UTF8.GetString(buffer, 0, read), Is.EqualTo("Hello, Tom!"));

            read = networkStream.Read(buffer, 0, buffer.Length);
            Assert.That(Encoding.UTF8.GetString(buffer, 0, read), Is.EqualTo("Hello, Ben!"));

            // Cleanup
            networkStream.Dispose();
            tcpClient.Close();
            tcpStubServer.Dispose();
        }

        [Test]
        public void AsyncTests()
        {
            // Arrange
            var tcpStubServer = new TcpStubServer(IPAddress.Any, 5000);

            tcpStubServer
                .When(bytes => true)
                .Return(async () =>
                {
                    await Task.Delay(50);
                    return Encoding.UTF8.GetBytes("Hello, World!");
                });

            var tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Loopback, 5000);

            var networkStream = tcpClient.GetStream();

            var message = new[] { byte.MinValue };

            networkStream.Write(message, 0, message.Length);

            // Act
            var buffer = new byte[8192];
            var read = networkStream.Read(buffer, 0, buffer.Length);

            // Assert
            Assert.That(Encoding.UTF8.GetString(buffer, 0, read), Is.EqualTo("Hello, World!"));

            // Cleanup
            networkStream.Dispose();
            tcpClient.Close();
            tcpStubServer.Dispose();
        }

        // ReSharper disable AccessToDisposedClosure
        [Test]
        public void NoResponseTests()
        {
            // Arrange
            var tcpStubServer = new TcpStubServer(IPAddress.Any, 5000);

            var tcpClient = new TcpClient()
            {
                Client = { ReceiveTimeout = 1000 }
            };
            tcpClient.Connect(IPAddress.Loopback, 5000);

            var networkStream = tcpClient.GetStream();

            var message = new[] { byte.MinValue };

            networkStream.Write(message, 0, message.Length);

            // Act
            var buffer = new byte[8192];
            TestDelegate testDelegate = () => networkStream.Read(buffer, 0, buffer.Length);

            // Assert
            Assert.Throws<IOException>(testDelegate);

            // Cleanup
            networkStream.Dispose();
            tcpClient.Close();
            tcpStubServer.Dispose();
        }
        // ReSharper restore AccessToDisposedClosure

        // ReSharper disable AccessToDisposedClosure
        [Test]
        public void NoRequestTests()
        {
            // Arrange
            var tcpStubServer = new TcpStubServer(IPAddress.Any, 5000);

            tcpStubServer
                .When(bytes => true)
                .Return(() => Encoding.UTF8.GetBytes("Hello, World!"));

            var tcpClient = new TcpClient()
            {
                Client = { ReceiveTimeout = 1000 }
            };
            tcpClient.Connect(IPAddress.Loopback, 5000);

            var networkStream = tcpClient.GetStream();

            var message = new byte[] {};

            networkStream.Write(message, 0, message.Length);

            // Act
            var buffer = new byte[8192];
            TestDelegate testDelegate = () => networkStream.Read(buffer, 0, buffer.Length);

            // Assert
            Assert.Throws<IOException>(testDelegate);

            // Cleanup
            networkStream.Dispose();
            tcpClient.Close();
            tcpStubServer.Dispose();
        }
        // ReSharper restore AccessToDisposedClosure
    }
}