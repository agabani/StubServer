using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using StubServer.Framework;

namespace StubServer.Smtp
{
    public partial class SmtpStubServer : IDisposable
    {
        private SmtpHandler _smtpHandler;

        public SmtpStubServer(IPAddress ipAddress, int port, Func<byte[]> initialResponse)
            : this(ipAddress, port, new [] { new Func<CancellationToken, Task<byte[]>>(token => Task.FromResult(initialResponse())) })
        {
        }

        public SmtpStubServer(IPAddress ipAddress, int port, Func<Task<byte[]>> initialResponse)
            : this(ipAddress, port, new[] { new Func<CancellationToken, Task<byte[]>>(token => initialResponse()) })
        {
        }

        public SmtpStubServer(IPAddress ipAddress, int port, Func<CancellationToken, Task<byte[]>> initialResponse)
            : this(ipAddress, port, new[] {initialResponse})
        {
        }

        public SmtpStubServer(IPAddress ipAddress, int port, IEnumerable<Func<byte[]>> initialResponses)
            : this(ipAddress, port, initialResponses.Select(func => new Func<CancellationToken, Task<byte[]>>(token => Task.FromResult(func()))))
        {
        }

        public SmtpStubServer(IPAddress ipAddress, int port, IEnumerable<Func<Task<byte[]>>> initialResponses)
            : this(ipAddress, port, initialResponses.Select(func => new Func<CancellationToken, Task<byte[]>>(token =>func())))
        {
        }

        public SmtpStubServer(IPAddress ipAddress, int port, IEnumerable<Func<CancellationToken, Task<byte[]>>> initialResponses)
        {
            _smtpHandler = new SmtpHandler(new TcpListener(ipAddress, port), initialResponses);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public MultipleReturn<byte[], byte[]> When(Expression<Func<byte[], bool>> expression)
        {
            return new MultipleReturn<byte[], byte[]>(_smtpHandler.AddSetup(expression));
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

    public partial class SmtpStubServer
    {
        [Obsolete(Literals.SetupIsDeprecatedPleaseUseWhenInstead)]
        public MultipleReturn<byte[], byte[]> Setup(Expression<Func<byte[], bool>> expression)
        {
            return When(expression);
        }
    }
}