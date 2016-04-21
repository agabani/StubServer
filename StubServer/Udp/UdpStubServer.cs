using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;

namespace StubServer.Udp
{
    public partial class UdpStubServer : IDisposable
    {
        private UdpHandler _udpHandler;

        public UdpStubServer(IPAddress ipAddress, int port)
        {
            _udpHandler = new UdpHandler(new UdpClient(new IPEndPoint(ipAddress, port)));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public MultipleReturn<byte[], byte[]> When(Expression<Func<byte[], bool>> expression)
        {
            return new MultipleReturn<byte[], byte[]>(_udpHandler.AddSetup(expression));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_udpHandler != null)
                {
                    _udpHandler.Dispose();
                    _udpHandler = null;
                }
            }
        }
    }

    public partial class UdpStubServer
    {
        [Obsolete(Literals.SetupIsDeprecatedPleaseUseWhenInstead)]
        public MultipleReturn<byte[], byte[]> Setup(Expression<Func<byte[], bool>> expression)
        {
            return When(expression);
        }
    }
}