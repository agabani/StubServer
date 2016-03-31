using System;
using System.Net;
using System.Net.Sockets;
using NUnit.Framework;
using StubServer.Tcp;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal abstract class TcpStubServerTests : IDisposable
    {
        protected ITcpStubServer TcpStubServer;
        private TcpClient _tcpClient;
        protected NetworkStream NetworkStream;

        [SetUp]
        public void SetUp()
        {
            TcpStubServer = new TcpStubServer(IPAddress.Loopback, 5053);
            _tcpClient = new TcpClient();
            _tcpClient.Connect(new IPEndPoint(IPAddress.Loopback, 5053));
            NetworkStream = _tcpClient.GetStream();
        }

        [TearDown]
        public void TearDown()
        {
            NetworkStream.Dispose();
            _tcpClient.Close();
            TcpStubServer.Dispose();
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
                if (NetworkStream != null)
                {
                    NetworkStream.Dispose();
                    NetworkStream = null;
                }

                if (_tcpClient != null)
                {
                    _tcpClient.Close();
                    _tcpClient = null;
                }

                if (NetworkStream != null)
                {
                    NetworkStream.Dispose();
                    NetworkStream = null;
                }
            }
        }
    }
}