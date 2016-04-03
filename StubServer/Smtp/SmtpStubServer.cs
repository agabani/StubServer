using System;
using System.Linq.Expressions;
using System.Net;

namespace StubServer.Smtp
{
    public class SmtpStubServer : ISmtpStubServer
    {
        private StubSmtpTcpListenerHandler _stubTcpListenerHandler;

        public SmtpStubServer(IPAddress ipAddress, int port)
        {
            _stubTcpListenerHandler = new StubSmtpTcpListenerHandler(new IPEndPoint(ipAddress, port));
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