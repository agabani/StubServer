using System;
using System.Net;
using System.Net.Sockets;
using NUnit.Framework;
using StubServer.Udp;

namespace StubServer.Tests.Acceptance.Udp
{
    internal abstract class UdpStubServerTests : IDisposable
    {
        protected IUdpStubServer UdpStubServer;
        protected UdpClient UdpClient;

        [SetUp]
        public void SetUp()
        {
            UdpStubServer = new UdpStubServer(IPAddress.Any, 5051);
            UdpClient = new UdpClient();
            UdpClient.Connect(IPAddress.Loopback, 5051);
        }

        [TearDown]
        public void TearDown()
        {
            UdpClient.Close();
            UdpStubServer.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UdpStubServer != null)
                {
                    UdpStubServer.Dispose();
                    UdpStubServer = null;
                }

                if (UdpClient != null)
                {
                    UdpClient.Close();
                    UdpClient = null;
                }
            }
        }
    }
}