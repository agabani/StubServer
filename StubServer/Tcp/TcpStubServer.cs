using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace StubServer.Tcp
{
    internal class StubTcpListener : TcpListener
    {
        private readonly List<Setup<byte[], byte[]>> _setups = new List<Setup<byte[], byte[]>>();

        internal StubTcpListener(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
        }

        internal void StartServer()
        {
            AcceptClientsAsync();
        }

        internal void StopServer()
        {
        }

        private async void AcceptClientsAsync()
        {
            while (true)
            {
                var tcpClient = await AcceptTcpClientAsync();

                var networkStream = tcpClient.GetStream();

                var buffer = new byte[256];
                var bytes = await networkStream.ReadAsync(buffer, 0, buffer.Length);

                foreach (var setup in _setups)
                {
                    var result = await setup.Result(buffer.Take(bytes).ToArray(), CancellationToken.None);

                    if (result != null)
                    {
                        var response = Encoding.UTF8.GetBytes("Hi!");
                        await networkStream.WriteAsync(response, 0, response.Length);
                    }
                }

                tcpClient.Close();
            }
        }

        internal ISetup<byte[]> AddSetup(Expression<Func<byte[], bool>> expression)
        {
            Setup<byte[], byte[]> udpSetup;
            _setups.Add(udpSetup = new Setup<byte[], byte[]>(expression));
            return udpSetup;
        }
    }

    public class TcpStubServer : ITcpStubServer
    {
        private StubTcpListener _tcpListener;

        public TcpStubServer(IPAddress ipAddress, int port)
        {
            _tcpListener = new StubTcpListener(new IPEndPoint(ipAddress, port));
            _tcpListener.Start();

            _tcpListener.StartServer();
        }

        public ISetup<byte[]> Setup(Expression<Func<byte[], bool>> expression)
        {
            return _tcpListener.AddSetup(expression);
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
        }
    }
}