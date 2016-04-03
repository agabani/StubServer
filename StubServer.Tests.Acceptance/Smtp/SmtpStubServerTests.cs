using System;
using System.Net;
using System.Net.Mail;
using StubServer.Smtp;

namespace StubServer.Tests.Acceptance.Smtp
{
    internal abstract class SmtpStubServerTests
    {
        protected ISmtpStubServer NewStubServer()
        {
            return new SmtpStubServer(IPAddress.Loopback, 5050);
        }

        protected SmtpClient NewSmtpClient()
        {
            return new SmtpClient("127.0.0.1", 5050);
        }

        protected void Cleanup(IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}