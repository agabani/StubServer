using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace StubServer.Tcp
{
    internal class StubTcpListener : TcpListener, IDisposable
    {
        private readonly List<Setup<byte[], byte[]>> _setups = new List<Setup<byte[], byte[]>>();

        internal StubTcpListener(IPEndPoint ipEndPoint) : base(ipEndPoint)
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
            var tcpListener = (TcpListener) ar.AsyncState;

            var tcpClient = tcpListener.EndAcceptTcpClient(ar);

            tcpListener.BeginAcceptTcpClient(RequestCallback, tcpListener);

            ProcessRequest(tcpClient);
        }

        private async void ProcessRequest(TcpClient tcpClient)
        {
            using (tcpClient)
            using (var networkStream = tcpClient.GetStream())
            {
                var buffer = new byte[8192];
                var bytes = networkStream.Read(buffer, 0, buffer.Length);

                var request = buffer.Take(bytes).ToArray();

                foreach (var setup in _setups)
                {
                    var result = await setup.Result(request, CancellationToken.None);

                    if (result != null)
                    {
                        networkStream.Write(result, 0, result.Length);
                    }
                }
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