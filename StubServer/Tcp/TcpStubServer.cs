using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;

namespace StubServer.Tcp
{
    public class TcpStubServer : ITcpStubServer
    {
        private TcpHandler _tcpHandler;

        public TcpStubServer(IPAddress ipAddress, int port)
        {
            _tcpHandler = new TcpHandler(new TcpListener(new IPEndPoint(ipAddress, port)));
        }

        public IResponse<byte[]> Setup(Expression<Func<byte[], bool>> expression)
        {
            return _tcpHandler.AddSetup(expression);
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
                if (_tcpHandler != null)
                {
                    _tcpHandler.Dispose();
                    _tcpHandler = null;
                }
            }
        }
    }
}