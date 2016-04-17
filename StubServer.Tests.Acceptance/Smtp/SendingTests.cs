using System.Net.Mail;
using System.Text;
using NUnit.Framework;

namespace StubServer.Tests.Acceptance.Smtp
{
    internal class SendingTests : SmtpStubServerTests
    {
        [Test]
        public void Should_accept_email()
        {
            // Arrange
            var smtpStubServer = NewStubServer();

            smtpStubServer
                .Setup(bytes => Encoding.ASCII.GetString(bytes).StartsWith("EHLO"))
                .Returns(() => Encoding.ASCII.GetBytes("250-smtp2.example.com Hello bob.example.org [192.0.2.201]\r\n"))
                .Then(() => Encoding.ASCII.GetBytes("250-SIZE 14680064\r\n"))
                .Then(() => Encoding.ASCII.GetBytes("250-PIPELINING\r\n"))
                .Then(() => Encoding.ASCII.GetBytes("250 HELP\r\n"));

            smtpStubServer
                .Setup(bytes => Encoding.ASCII.GetString(bytes).Equals("MAIL FROM:<jane@contoso.com>\r\n"))
                .Returns(() => Encoding.ASCII.GetBytes("250 Ok\r\n"));

            smtpStubServer
                .Setup(bytes => Encoding.ASCII.GetString(bytes).Equals("RCPT TO:<ben@contoso.com>\r\n"))
                .Returns(() => Encoding.ASCII.GetBytes("250 Ok\r\n"));

            smtpStubServer
                .Setup(bytes => Encoding.ASCII.GetString(bytes).Equals("DATA\r\n"))
                .Returns(() => Encoding.ASCII.GetBytes("354 End data with <CR><LF>.<CR><LF>\r\n"));

            smtpStubServer
                .Setup(bytes => Encoding.ASCII.GetString(bytes).Contains("\r\n.\r\n"))
                .Returns(() => Encoding.ASCII.GetBytes("250 Ok: queued as 12345\r\n"));

            smtpStubServer
                .Setup(bytes => Encoding.ASCII.GetString(bytes).Equals("QUIT\r\n"))
                .Returns(() => Encoding.ASCII.GetBytes("221 Bye\r\n"));

            var smtpClient = NewSmtpClient();

            var message = new MailMessage(
                new MailAddress("jane@contoso.com", "Jane Clayton"),
                new MailAddress("ben@contoso.com"))
            {
                Body = "This is a test e-mail message sent by an application.",
                BodyEncoding = Encoding.UTF8,
                Subject = "test message 1",
                SubjectEncoding = Encoding.UTF8
            };

            // Act
            TestDelegate testDelegate = () => smtpClient.Send(message);

            // Assert
            Assert.DoesNotThrow(testDelegate);

            // Cleanup
            Cleanup(message);
            Cleanup(smtpClient);
            Cleanup(smtpStubServer);
        }
    }
}