using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using StubServer.Smtp.StateObjects;

namespace StubServer.Smtp
{
    internal class StubSmtpHandler : IDisposable
    {
        private readonly List<Setup<byte[], byte[]>> _setups = new List<Setup<byte[], byte[]>>();
        private TcpListener _tcpListener;

        internal StubSmtpHandler(TcpListener tcpListener)
        {
            _tcpListener = tcpListener;
            _tcpListener.Start();
            _tcpListener.BeginAcceptSocket(AcceptSocketCallback, new TcpListenerStateObject(_tcpListener, _setups));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static void AcceptSocketCallback(IAsyncResult ar)
        {
            var tcpListenerStateObject = (TcpListenerStateObject) ar.AsyncState;

            var tcpListener = tcpListenerStateObject.TcpListener;
            var setups = tcpListenerStateObject.Setups;

            var socket = tcpListener.EndAcceptSocket(ar);

            tcpListener.BeginAcceptSocket(AcceptSocketCallback, ar);

            var socketStateObject = new SocketStateObject(socket, setups);

            socketStateObject.Socket.Send(Encoding.UTF8.GetBytes("220 SMTP StubServer\r\n"));

            socketStateObject.Socket.BeginReceive(socketStateObject.Buffer, 0, socketStateObject.Buffer.Length,
                SocketFlags.None, RecieveCallback, socketStateObject);
        }

        private static async void RecieveCallback(IAsyncResult ar)
        {
            var stateObject = (SocketStateObject) ar.AsyncState;

            var endReceive = stateObject.Socket.EndReceive(ar);

            var request = stateObject.Buffer
                .Take(endReceive)
                .ToArray();

            foreach (var setup in stateObject.Setups)
            {
                var result = await setup.Result(request, CancellationToken.None);

                if (result != null)
                {
                    stateObject.Socket.Send(result);
                    break;
                }
            }

            stateObject.Socket.BeginReceive(stateObject.Buffer, 0, stateObject.Buffer.Length,
                SocketFlags.None, RecieveCallback, stateObject);
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
                if (_tcpListener != null)
                {
                    _tcpListener.Stop();
                    _tcpListener = null;
                }
            }
        }
    }
}