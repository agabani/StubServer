using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;

namespace StubServer.Smtp
{
    public class SmtpStubServer : ISmtpStubServer
    {
        private SmtpHandler _smtpHandler;

        public SmtpStubServer(IPAddress ipAddress, int port)
        {
            _smtpHandler = new SmtpHandler(new TcpListener(ipAddress, port));
        }

        public ISetup<byte[]> Setup(Expression<Func<byte[], bool>> expression)
        {
            return _smtpHandler.AddSetup(expression);
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
                if (_smtpHandler != null)
                {
                    _smtpHandler.Dispose();
                    _smtpHandler = null;
                }
            }
        }
    }
}