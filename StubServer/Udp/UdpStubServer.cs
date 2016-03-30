using System;
using System.Linq.Expressions;
using System.Net;

namespace StubServer.Udp
{
    public class UdpStubServer : IUdpStubServer
    {
        private StubUdpClientHandler _stubUdpClientHandler;

        public UdpStubServer(IPAddress ipAddress, int port)
        {
            _stubUdpClientHandler = new StubUdpClientHandler(new IPEndPoint(ipAddress, port));
        }

        public ISetup<byte[]> Setup(Expression<Func<byte[], bool>> expression)
        {
            return _stubUdpClientHandler.AddSetup(expression);
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
                if (_stubUdpClientHandler != null)
                {
                    _stubUdpClientHandler.Close();
                    _stubUdpClientHandler = null;
                }
            }
        }
    }
}