using System;
using System.Linq.Expressions;
using System.Net;

namespace StubServer.Tcp
{
    public class TcpStubServer : ITcpStubServer
    {
        private StubTcpListener _stubTcpListener;

        public TcpStubServer(IPAddress ipAddress, int port)
        {
            _stubTcpListener = new StubTcpListener(new IPEndPoint(ipAddress, port));
        }

        public ISetup<byte[]> Setup(Expression<Func<byte[], bool>> expression)
        {
            return _stubTcpListener.AddSetup(expression);
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
                if (_stubTcpListener != null)
                {
                    _stubTcpListener.Dispose();
                    _stubTcpListener = null;
                }
            }
        }
    }
}