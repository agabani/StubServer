# StubServer

> Quickly stub out 3rd party servers to be used against your automated development test cycle.

> Designed to feel familiar to use, without having restrictions forced upon you.

> Can be as simple as you want... Or as complex...

[![NuGet downloads](https://img.shields.io/badge/nuget-v0.1.5-blue.svg)](https://www.nuget.org/packages/StubServer)

Checkout the [Wiki](https://github.com/agabani/StubServer/wiki) for more examples!

### Example HTTP StubServer
```csharp
// Arrange
IHttpStubServer httpStubServer = new HttpStubServer(new Uri("http://localhost:5000"));

httpStubServer
	.Setup(message => message.RequestUri.PathAndQuery.Equals("/JohnSmith"))
	.Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

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
ITcpStubServer tcpStubServer = new TcpStubServer(IPAddress.Any, 5000);

tcpStubServer
	.Setup(bytes => Encoding.UTF8.GetString(bytes).Equals("Hi!"))
	.Returns(() => Encoding.UTF8.GetBytes("Hello, World!"));

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
IUdpStubServer udpStubServer = new UdpStubServer(IPAddress.Any, 5000);

udpStubServer
	.Setup(bytes => Encoding.UTF8.GetString(bytes).Equals("Hi!"))
	.Returns(() => Encoding.UTF8.GetBytes("Hello, World!"));

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