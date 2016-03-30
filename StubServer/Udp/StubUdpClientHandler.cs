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
        private readonly List<UdpSetup> _setups = new List<UdpSetup>();

        public StubUdpClientHandler(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
            BeginReceive(RequestCallback, new ObjectState(this, ipEndPoint));
        }

        private void RequestCallback(IAsyncResult ar)
        {
            var objectState = (ObjectState) ar.AsyncState;
            var ipEndPoint = objectState.IpEndPoint;

            var endReceiveBytes = objectState.UdpClient.EndReceive(ar, ref ipEndPoint);

            foreach (var setup in _setups)
            {
                var result = setup.Result(endReceiveBytes, CancellationToken.None).Result;

                if (result != null)
                {
                    objectState.UdpClient.Send(result, result.Length, ipEndPoint);
                    return;
                }
            }
        }

        internal ISetup<byte[]> AddSetup(Expression<Func<byte[], bool>> expression)
        {
            UdpSetup udpSetup;
            _setups.Add(udpSetup = new UdpSetup(expression));
            return udpSetup;
        }

        private class ObjectState
        {
            public ObjectState(UdpClient udpClient, IPEndPoint ipEndPoint)
            {
                UdpClient = udpClient;
                IpEndPoint = ipEndPoint;
            }

            public UdpClient UdpClient { get; }
            public IPEndPoint IpEndPoint { get; }
        }
    }
}