using System.Collections.Generic;
using System.Net.Sockets;

namespace StubServer.Smtp.StateObjects
{
    internal class TcpListenerStateObject
    {
        internal TcpListenerStateObject(TcpListener tcpListener, List<Setup<byte[], byte[]>> setups)
        {
            TcpListener = tcpListener;
            Setups = setups;
        }

        internal TcpListener TcpListener { get; private set; }
        internal List<Setup<byte[], byte[]>> Setups { get; private set; }
    }
}