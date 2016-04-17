using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer.Smtp
{
    public class SmtpStubServer : IDisposable
    {
        private SmtpHandler _smtpHandler;

        public SmtpStubServer(IPAddress ipAddress, int port, Func<byte[]> initialResponse)
            : this(ipAddress, port, token => Task.FromResult(initialResponse()))
        {
        }

        public SmtpStubServer(IPAddress ipAddress, int port, Func<Task<byte[]>> initialResponse)
            : this(ipAddress, port, token => initialResponse())
        {
        }

        public SmtpStubServer(IPAddress ipAddress, int port, Func<CancellationToken, Task<byte[]>> initialResponse)
        {
            _smtpHandler = new SmtpHandler(new TcpListener(ipAddress, port), initialResponse);
        }

        public IMultipleReturns<byte[]> Setup(Expression<Func<byte[], bool>> expression)
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