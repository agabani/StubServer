using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace StubServer.Udp
{
    public class UdpStubServer
    {
        private UdpClient _udpClient;
        private IPEndPoint _ipEndPoint;

        public UdpStubServer()
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Any, 5051);
            _udpClient = new UdpClient(_ipEndPoint);

            _udpClient.BeginReceive(RequestCallback, new ObjectState(_udpClient, _ipEndPoint));
        }

        private void RequestCallback(IAsyncResult ar)
        {
            var objectState = (ObjectState) ar.AsyncState;
            var ipEndPoint = objectState.IpEndPoint;

            var endReceiveBytes = objectState.UdpClient.EndReceive(ar, ref ipEndPoint);

            var receive = Encoding.UTF8.GetString(endReceiveBytes);

            if (receive == "Hello, World!")
            {
                var bytes = Encoding.UTF8.GetBytes("John Smith");

                objectState.UdpClient.Send(bytes, bytes.Length, ipEndPoint);
            }
        }

        private class ObjectState
        {
            public ObjectState(UdpClient udpClient, IPEndPoint ipEndPoint)
            {
                UdpClient = udpClient;
                IpEndPoint = ipEndPoint;
            }

            public UdpClient UdpClient { get; private set; }
            public IPEndPoint IpEndPoint { get; private set; }
        }
    }
}