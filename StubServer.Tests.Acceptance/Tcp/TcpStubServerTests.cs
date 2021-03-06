﻿using System;
using System.Net;
using System.Net.Sockets;
using StubServer.Tcp;

namespace StubServer.Tests.Acceptance.Tcp
{
    internal abstract class TcpStubServerTests
    {
        protected TcpStubServer NewStubServer()
        {
            return new TcpStubServer(IPAddress.Loopback, 5050);
        }

        protected TcpClient NewTcpClient()
        {
            var tcpClient = new TcpClient
            {
                Client = {ReceiveTimeout = (int) TimeSpan.FromSeconds(1).TotalMilliseconds}
            };
            tcpClient.Connect(new IPEndPoint(IPAddress.Loopback, 5050));
            return tcpClient;
        }

        protected void Cleanup(IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}