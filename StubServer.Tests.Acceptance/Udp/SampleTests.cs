using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StubServer.Udp;

namespace StubServer.Tests.Acceptance.Udp
{
    internal class SampleTests
    {
        [Test]
        public void SimpleTest()
        {
            // Arrange
            var udpStubServer = new UdpStubServer(IPAddress.Any, 5000);

            udpStubServer
                .When(bytes => true)
                .Return(() => Encoding.UTF8.GetBytes("Hello, World!"));

            var udpClient = new UdpClient();
            udpClient.Connect(IPAddress.Loopback, 5000);

            var message = new byte[] {};

            udpClient.Send(message, message.Length);

            // Act
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var receive = udpClient.Receive(ref ipEndPoint);

            // Assert
            Assert.That(Encoding.UTF8.GetString(receive), Is.EqualTo("Hello, World!"));

            // Cleanup
            udpStubServer.Dispose();
            udpClient.Close();
        }

        [Test]
        public void DemoTest()
        {
            // Arrange
            var udpStubServer = new UdpStubServer(IPAddress.Any, 5000);

            udpStubServer
                .When(bytes => Encoding.UTF8.GetString(bytes).Equals("Hi!"))
                .Return(() => Encoding.UTF8.GetBytes("Hello, World!"));

            var udpClient = new UdpClient();
            udpClient.Connect(IPAddress.Loopback, 5000);

            var message = Encoding.UTF8.GetBytes("Hi!");

            udpClient.Send(message, message.Length);

            // Act
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var receive = udpClient.Receive(ref ipEndPoint);

            // Assert
            Assert.That(Encoding.UTF8.GetString(receive), Is.EqualTo("Hello, World!"));

            // Cleanup
            udpStubServer.Dispose();
            udpClient.Close();
        }

        [Test]
        public void ChainedResponses()
        {
            // Arrange
            var udpStubServer = new UdpStubServer(IPAddress.Any, 5000);

            udpStubServer
                .When(bytes => Encoding.UTF8.GetString(bytes).Equals("Hi!"))
                .Return(() => Encoding.UTF8.GetBytes("Hello, John!"))
                .Return(() => Encoding.UTF8.GetBytes("Hello, Tom!"))
                .Return(() => Encoding.UTF8.GetBytes("Hello, Ben!"));

            var udpClient = new UdpClient();
            udpClient.Connect(IPAddress.Loopback, 5000);

            var message = Encoding.UTF8.GetBytes("Hi!");

            // Act & Assert
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

            udpClient.Send(message, message.Length);
            var result = Encoding.UTF8.GetString(udpClient.Receive(ref ipEndPoint));
            Assert.That(result, Is.EqualTo("Hello, John!"));

            udpClient.Send(message, message.Length);
            result = Encoding.UTF8.GetString(udpClient.Receive(ref ipEndPoint));
            Assert.That(result, Is.EqualTo("Hello, Tom!"));

            udpClient.Send(message, message.Length);
            result = Encoding.UTF8.GetString(udpClient.Receive(ref ipEndPoint));
            Assert.That(result, Is.EqualTo("Hello, Ben!"));

            udpClient.Send(message, message.Length);
            result = Encoding.UTF8.GetString(udpClient.Receive(ref ipEndPoint));
            Assert.That(result, Is.EqualTo("Hello, Ben!"));

            // Cleanup
            udpStubServer.Dispose();
            udpClient.Close();
        }

        [Test]
        public void MultipleResponse()
        {
            // Arrange
            var udpStubServer = new UdpStubServer(IPAddress.Any, 5000);

            udpStubServer
                .When(bytes => Encoding.UTF8.GetString(bytes).Equals("Hi!"))
                .Return(() => Encoding.UTF8.GetBytes("Hello, John!"))
                .Then(() => Encoding.UTF8.GetBytes("Hello, Tom!"))
                .Then(() => Encoding.UTF8.GetBytes("Hello, Ben!"));

            var udpClient = new UdpClient();
            udpClient.Connect(IPAddress.Loopback, 5000);

            var message = Encoding.UTF8.GetBytes("Hi!");

            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // Act 
            udpClient.Send(message, message.Length);

            // Assert
            var result = Encoding.UTF8.GetString(udpClient.Receive(ref ipEndPoint));
            Assert.That(result, Is.EqualTo("Hello, John!"));

            result = Encoding.UTF8.GetString(udpClient.Receive(ref ipEndPoint));
            Assert.That(result, Is.EqualTo("Hello, Tom!"));

            result = Encoding.UTF8.GetString(udpClient.Receive(ref ipEndPoint));
            Assert.That(result, Is.EqualTo("Hello, Ben!"));

            // Cleanup
            udpStubServer.Dispose();
            udpClient.Close();
        }

        [Test]
        public void AsyncTests()
        {
            // Arrange
            var udpStubServer = new UdpStubServer(IPAddress.Any, 5000);

            udpStubServer
                .When(bytes => true)
                .Return(async () =>
                {
                    await Task.Delay(50);
                    return Encoding.UTF8.GetBytes("Hello, World!");
                });

            var udpClient = new UdpClient();
            udpClient.Connect(IPAddress.Loopback, 5000);

            var message = new byte[] { };

            udpClient.Send(message, message.Length);

            // Act
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var receive = udpClient.Receive(ref ipEndPoint);

            // Assert
            Assert.That(Encoding.UTF8.GetString(receive), Is.EqualTo("Hello, World!"));

            // Cleanup
            udpStubServer.Dispose();
            udpClient.Close();
        }

        [Test]
        public void NoResponseTests()
        {
            // Arrange
            var udpStubServer = new UdpStubServer(IPAddress.Any, 5000);

            var udpClient = new UdpClient
            {
                Client = {ReceiveTimeout = 1000}
            };
            udpClient.Connect(IPAddress.Loopback, 5000);

            var message = Encoding.UTF8.GetBytes("Incorrect message");

            udpClient.Send(message, message.Length);

            // Act
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            TestDelegate testDelegate = () => udpClient.Receive(ref ipEndPoint);

            // Assert
            Assert.Throws<SocketException>(testDelegate);

            // Cleanup
            udpStubServer.Dispose();
            udpClient.Close();
        }
    }
}