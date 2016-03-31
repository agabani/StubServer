using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace StubServer.Udp
{
    internal class StubUdpClientHandler : UdpClient
    {
        private readonly List<Setup<byte[], byte[]>> _setups = new List<Setup<byte[], byte[]>>();

        internal StubUdpClientHandler(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
            BeginReceive(RequestCallback, ipEndPoint);
        }

        private void RequestCallback(IAsyncResult ar)
        {
            var ipEndPoint = (IPEndPoint) ar.AsyncState;

            var bytes = EndReceive(ar, ref ipEndPoint);

            BeginReceive(RequestCallback, ipEndPoint);

            ProcessRequest(bytes, ipEndPoint);
        }

        private async void ProcessRequest(byte[] bytes, IPEndPoint ipEndPoint)
        {
            foreach (var setup in _setups)
            {
                var result = await setup.Result(bytes, CancellationToken.None);

                if (result != null)
                {
                    Send(result, result.Length, ipEndPoint);
                    return;
                }
            }
        }

        internal ISetup<byte[]> AddSetup(Expression<Func<byte[], bool>> expression)
        {
            Setup<byte[], byte[]> udpSetup;
            _setups.Add(udpSetup = new Setup<byte[], byte[]>(expression));
            return udpSetup;
        }
    }
}