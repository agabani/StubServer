# StubServer

> Quickly stub out 3rd party servers to be used against your automated development test cycle.

> Designed to feel familiar to use, without having restrictions forced upon you.

> Can be as simple as you want... Or as complex...

[![NuGet downloads](https://img.shields.io/badge/nuget-v0.2.1.0-blue.svg)](https://www.nuget.org/packages/StubServer)

Checkout the [Wiki](https://github.com/agabani/StubServer/wiki) for more examples!

### Example HTTP StubServer
```csharp
// Arrange
var httpStubServer = new HttpStubServer(new Uri("http://localhost:5000"));

httpStubServer
	.When(message => message.RequestUri.PathAndQuery.Equals("/JohnSmith"))
	.Return(() => new HttpResponseMessage(HttpStatusCode.OK));

var httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:5000")};

// Act
var httpResponseMessage = httpClient.GetAsync("/JohnSmith")
	.GetAwaiter().GetResult();

// Assert
Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));

// Clean Up
httpStubServer.Dispose();
```

### Example TCP StubServer
```csharp
// Arrange
var tcpStubServer = new TcpStubServer(IPAddress.Any, 5000);

tcpStubServer
	.When(bytes => Encoding.UTF8.GetString(bytes).Equals("Hi!"))
	.Return(() => Encoding.UTF8.GetBytes("Hello, World!"));

var tcpClient = new TcpClient();
tcpClient.Connect(IPAddress.Loopback, 5000);

var networkStream = tcpClient.GetStream();

var message = Encoding.UTF8.GetBytes("Hi!");

networkStream.Write(message, 0, message.Length);

// Act
var buffer = new byte[8192];
var read = networkStream.Read(buffer, 0, buffer.Length);

// Assert
Assert.That(Encoding.UTF8.GetString(buffer, 0, read), Is.EqualTo("Hello, World!"));

// Cleanup
networkStream.Dispose();
tcpClient.Close();
tcpStubServer.Dispose();
```

### Example UDP StubServer
```csharp
// Arrange
var udpStubServer = new UdpStubServer(IPAddress.Any, 5000);

udpStubServer
	.When(bytes => Encoding.UTF8.GetString(bytes).Equals("Hi!"))
	.Return(() => Encoding.UTF8.GetBytes("Hello, World!"));

var udpClient = new UdpClient();
udpClient.Connect(IPAddress.Loopback, 5000);

var message = Encoding.UTF8.GetBytes("Hi!");

udpClient.Send(message, message.Length);

// Act
var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
var receive = udpClient.Receive(ref ipEndPoint);

// Assert
Assert.That(Encoding.UTF8.GetString(receive), Is.EqualTo("Hello, World!"));

// Cleanup
udpStubServer.Dispose();
udpClient.Close();
```

### Example SMTP StubServer
```csharp
// Arrange
var smtpStubServer = new SmtpStubServer(IPAddress.Loopback, 5000,
	() => Encoding.ASCII.GetBytes("220 SMTP StubServer\r\n"));

smtpStubServer
	.When(bytes => Encoding.ASCII.GetString(bytes).StartsWith("EHLO"))
	.Return(() => Encoding.ASCII.GetBytes("250-smtp.example.com Hello www.example.org [123.0.0.321]\r\n"))
	.Then(() => Encoding.ASCII.GetBytes("250-SIZE 14680064\r\n"))
	.Then(() => Encoding.ASCII.GetBytes("250-PIPELINING\r\n"))
	.Then(() => Encoding.ASCII.GetBytes("250 HELP\r\n"));

smtpStubServer
	.When(bytes => Encoding.ASCII.GetString(bytes).Equals("MAIL FROM:<jane@contoso.com>\r\n"))
	.Return(() => Encoding.ASCII.GetBytes("250 Ok\r\n"));

smtpStubServer
	.When(bytes => Encoding.ASCII.GetString(bytes).Equals("RCPT TO:<ben@contoso.com>\r\n"))
	.Return(() => Encoding.ASCII.GetBytes("250 Ok\r\n"));

smtpStubServer
	.When(bytes => Encoding.ASCII.GetString(bytes).Equals("DATA\r\n"))
	.Return(() => Encoding.ASCII.GetBytes("354 End data with <CR><LF>.<CR><LF>\r\n"));

smtpStubServer
	.When(bytes => Encoding.ASCII.GetString(bytes).Contains("\r\n.\r\n"))
	.Return(() => Encoding.ASCII.GetBytes("250 Ok: queued as 12345\r\n"));

smtpStubServer
	.When(bytes => Encoding.ASCII.GetString(bytes).Equals("QUIT\r\n"))
	.Return(() => Encoding.ASCII.GetBytes("221 Bye\r\n"));

var smtpClient = new SmtpClient("127.0.0.1", 5000);

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
message.Dispose();
smtpClient.Dispose();
smtpStubServer.Dispose();
```