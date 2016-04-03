using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace StubServer.Smtp
{
    internal class StubSmtpTcpListenerHandler : TcpListener, IDisposable
    {
        private readonly List<Setup<byte[], byte[]>> _setups = new List<Setup<byte[], byte[]>>();

        internal StubSmtpTcpListenerHandler(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
            Start();
            BeginAcceptTcpClient(RequestCallback, this);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void RequestCallback(IAsyncResult ar)
        {
            if (!Active)
            {
                return;
            }

            var tcpClient = EndAcceptTcpClient(ar);

            BeginAcceptTcpClient(RequestCallback, this);

            ProcessRequest(tcpClient);
        }

        private async void ProcessRequest(TcpClient tcpClient)
        {
            var networkStream = tcpClient.GetStream();
            {
                var bytes = Encoding.UTF8.GetBytes("220 SMTP StubServer\r\n");
                networkStream.Write(bytes, 0, bytes.Length);

                var buffer = new byte[8192];

                do
                {
                    var request = buffer
                        .Take(networkStream.Read(buffer, 0, buffer.Length))
                        .ToArray();

                    if (request.Length == 0)
                    {
                        return;
                    }

                    foreach (var setup in _setups)
                    {
                        var result = await setup.Result(request, CancellationToken.None);

                        if (result != null)
                        {
                            networkStream.Write(result, 0, result.Length);
                        }
                    }
                } while (tcpClient.Connected);
            }
        }

        internal ISetup<byte[]> AddSetup(Expression<Func<byte[], bool>> expression)
        {
            Setup<byte[], byte[]> udpSetup;
            _setups.Add(udpSetup = new Setup<byte[], byte[]>(expression));
            return udpSetup;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Active)
                {
                    Stop();
                }
            }
        }
    }
}