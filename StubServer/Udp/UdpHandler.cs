using System;
using System.Collections.Generic;
using System.Linq;
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

                foreach (var results in _setups.Select(setup => setup
                    .Results(udpReceiveResult.Buffer, CancellationToken.None))
                    .Where(results => results != null))
                {
                    foreach (var task in results)
                    {
                        var bytes = await task;

                        await _udpClient.SendAsync(bytes, bytes.Length, udpReceiveResult.RemoteEndPoint);
                    }

                    break;
                }
            }
        }

        internal IMultipleReturns<byte[]> AddSetup(Expression<Func<byte[], bool>> expression)
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