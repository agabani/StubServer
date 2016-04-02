using System;
using System.Linq.Expressions;
using System.Net;

namespace StubServer.Tcp
{
    public class TcpStubServer : ITcpStubServer
    {
        private StubTcpListenerHandler _stubTcpListenerHandler;

        public TcpStubServer(IPAddress ipAddress, int port)
        {
            _stubTcpListenerHandler = new StubTcpListenerHandler(new IPEndPoint(ipAddress, port));
        }

        public ISetup<byte[]> Setup(Expression<Func<byte[], bool>> expression)
        {
            return _stubTcpListenerHandler.AddSetup(expression);
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
                if (_stubTcpListenerHandler != null)
                {
                    _stubTcpListenerHandler.Dispose();
                    _stubTcpListenerHandler = null;
                }
            }
        }
    }
}