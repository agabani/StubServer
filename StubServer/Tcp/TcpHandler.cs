using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading;

namespace StubServer.Tcp
{
    internal class TcpHandler
    {
        private readonly List<Setup<byte[], byte[]>> _setups = new List<Setup<byte[], byte[]>>();
        private bool _disposed;
        private TcpListener _tcpListener;

        public TcpHandler(TcpListener tcpListener)
        {
            _tcpListener = tcpListener;
            _tcpListener.Start();
            HandleIncomingClients();
        }

        private async void HandleIncomingClients()
        {
            while (!_disposed)
            {
                var tcpClient = await _tcpListener
                    .AcceptTcpClientAsync()
                    .ConfigureAwait(false);

                HandleIncomingRequests(tcpClient);
            }
        }

        private async void HandleIncomingRequests(TcpClient tcpClient)
        {
            using (tcpClient)
            using (var networkStream = tcpClient.GetStream())
            {
                var buffer = new byte[8192];

                do
                {
                    var request = buffer
                        .Take(await networkStream
                            .ReadAsync(buffer, 0, buffer.Length)
                            .ConfigureAwait(false))
                        .ToArray();

                    foreach (var setup in _setups)
                    {
                        var result = await setup
                            .Result(request, CancellationToken.None)
                            .ConfigureAwait(false);

                        if (result != null)
                        {
                            await networkStream
                                .WriteAsync(result, 0, result.Length)
                                .ConfigureAwait(false);
                        }
                    }
                } while (tcpClient.Connected);
            }
        }

        internal ISetup<byte[]> AddSetup(Expression<Func<byte[], bool>> expression)
        {
            Setup<byte[], byte[]> setup;
            _setups.Add(setup = new Setup<byte[], byte[]>(expression));
            return setup;
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
                if (_tcpListener != null)
                {
                    _tcpListener.Stop();
                    _tcpListener = null;
                }
            }

            _disposed = true;
        }
    }
}