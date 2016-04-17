using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;

namespace StubServer.Udp
{
    public class UdpStubServer : IUdpStubServer
    {
        private UdpHandler _udpHandler;

        public UdpStubServer(IPAddress ipAddress, int port)
        {
            _udpHandler = new UdpHandler(new UdpClient(new IPEndPoint(ipAddress, port)));
        }

        public IResponse<byte[]> Setup(Expression<Func<byte[], bool>> expression)
        {
            return _udpHandler.AddSetup(expression);
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
                if (_udpHandler != null)
                {
                    _udpHandler.Dispose();
                    _udpHandler = null;
                }
            }
        }
    }
}