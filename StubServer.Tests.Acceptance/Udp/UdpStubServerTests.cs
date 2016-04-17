using System;
using System.Net;
using System.Net.Sockets;
using StubServer.Udp;

namespace StubServer.Tests.Acceptance.Udp
{
    internal abstract class UdpStubServerTests
    {
        protected UdpStubServer NewStubServer()
        {
            return new UdpStubServer(IPAddress.Any, 5050);
        }

        protected UdpClient NewUdpClient()
        {
            var udpClient = new UdpClient
            {
                Client = {ReceiveTimeout = (int) TimeSpan.FromSeconds(1).TotalMilliseconds}
            };
            udpClient.Connect(IPAddress.Loopback, 5050);
            return udpClient;
        }

        protected void Cleanup(UdpClient udpClient)
        {
            udpClient.Close();
        }

        protected void Cleanup(IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}