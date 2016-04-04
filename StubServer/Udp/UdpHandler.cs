using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading;

namespace StubServer.Udp
{
    internal class UdpHandler : IDisposable
    {
        private readonly List<Setup<byte[], byte[]>> _setups = new List<Setup<byte[], byte[]>>();

        private bool _disposed;
        private UdpClient _udpClient;

        internal UdpHandler(UdpClient udpClient)
        {
            _udpClient = udpClient;
            HandleIncomingRequests();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async void HandleIncomingRequests()
        {
            while (!_disposed)
            {
                var udpReceiveResult = await _udpClient.ReceiveAsync();

                foreach (var setup in _setups)
                {
                    var result = await setup
                        .Result(udpReceiveResult.Buffer, CancellationToken.None)
                        .ConfigureAwait(false);

                    if (result != null)
                    {
                        await _udpClient
                            .SendAsync(result, result.Length, udpReceiveResult.RemoteEndPoint)
                            .ConfigureAwait(false);

                        break;
                    }
                }
            }
        }

        internal ISetup<byte[]> AddSetup(Expression<Func<byte[], bool>> expression)
        {
            Setup<byte[], byte[]> setup;
            _setups.Add(setup = new Setup<byte[], byte[]>(expression));
            return setup;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_udpClient != null)
                {
                    _udpClient.Close();
                    _udpClient = null;
                }
            }

            _disposed = true;
        }
    }
}