using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;

namespace StubServer.Smtp
{
    public class SmtpStubServer : ISmtpStubServer
    {
        private StubSmtpHandler _stubSmtpHandler;

        public SmtpStubServer(IPAddress ipAddress, int port)
        {
            _stubSmtpHandler = new StubSmtpHandler(new TcpListener(ipAddress, port));
        }

        public ISetup<byte[]> Setup(Expression<Func<byte[], bool>> expression)
        {
            return _stubSmtpHandler.AddSetup(expression);
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
                if (_stubSmtpHandler != null)
                {
                    _stubSmtpHandler.Dispose();
                    _stubSmtpHandler = null;
                }
            }
        }
    }
}