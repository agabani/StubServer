using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StubServer.Smtp;

namespace StubServer.Tests.Acceptance.Smtp
{
    internal abstract class SmtpStubServerTests
    {
        protected SmtpStubServer NewStubServer()
        {
            return new SmtpStubServer(IPAddress.Loopback, 5050, new Func<CancellationToken, Task<byte[]>>[]
            {
                token => Task.FromResult(Encoding.ASCII.GetBytes("220 SMTP StubServer\r\n"))
            });
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