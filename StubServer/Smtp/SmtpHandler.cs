using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer.Smtp
{
    internal class SmtpHandler : IDisposable
    {
        private readonly IEnumerable<Func<CancellationToken, Task<byte[]>>> _initialResponses;
        private readonly List<Setup<byte[], byte[]>> _setups = new List<Setup<byte[], byte[]>>();
        private bool _disposed;
        private TcpListener _tcpListener;

        internal SmtpHandler(TcpListener tcpListener, IEnumerable<Func<CancellationToken, Task<byte[]>>> initialResponses)
        {
            _tcpListener = tcpListener;
            _initialResponses = initialResponses;
            _tcpListener.Start();
            HandleIncomingClients();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                foreach (var initalResponse in _initialResponses)
                {
                    var bytes = await initalResponse(CancellationToken.None)
                    .ConfigureAwait(false);

                    await networkStream
                        .WriteAsync(bytes, 0, bytes.Length)
                        .ConfigureAwait(false);
                }

                var buffer = new byte[8192];

                do
                {
                    var request = buffer
                        .Take(await networkStream
                            .ReadAsync(buffer, 0, buffer.Length)
                            .ConfigureAwait(false))
                        .ToArray();

                    if (request.Length == 0)
                    {
                        return;
                    }

                    foreach (var results in _setups.Select(setup => setup
                        .Results(request, CancellationToken.None))
                        .Where(results => results != null))
                    {
                        foreach (var task in results)
                        {
                            var result = await task.ConfigureAwait(false);

                            await networkStream
                                .WriteAsync(result, 0, result.Length)
                                .ConfigureAwait(false);
                        }

                        break;
                    }
                } while (tcpClient.Connected);
            }
        }

        internal Setup<byte[], byte[]> AddSetup(Expression<Func<byte[], bool>> expression)
        {
            Setup<byte[], byte[]> setup;
            _setups.Add(setup = new Setup<byte[], byte[]>(expression));
            return setup;
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