﻿using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;

namespace StubServer.Tcp
{
    public partial class TcpStubServer : IDisposable
    {
        private TcpHandler _tcpHandler;

        public TcpStubServer(IPAddress ipAddress, int port)
        {
            _tcpHandler = new TcpHandler(new TcpListener(new IPEndPoint(ipAddress, port)));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public MultipleReturn<byte[], byte[]> When(Expression<Func<byte[], bool>> expression)
        {
            return new MultipleReturn<byte[], byte[]>(_tcpHandler.AddSetup(expression));
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

    public partial class TcpStubServer
    {
        [Obsolete(Literals.SetupIsDeprecatedPleaseUseWhenInstead)]
        public MultipleReturn<byte[], byte[]> Setup(Expression<Func<byte[], bool>> expression)
        {
            return When(expression);
        }
    }
}