using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace StubServer.Smtp.StateObjects
{
    internal class SocketStateObject : IDisposable
    {
        internal SocketStateObject(Socket socket, IReadOnlyList<Setup<byte[], byte[]>> setups)
        {
            Socket = socket;
            Buffer = new byte[8192];
            Setups = setups;
        }

        internal Socket Socket { get; private set; }
        internal byte[] Buffer { get; private set; }
        internal IReadOnlyList<Setup<byte[], byte[]>> Setups { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Socket != null)
                {
                    Socket.Dispose();
                    Socket = null;
                }
            }
        }
    }
}