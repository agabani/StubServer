# StubServer

> Quickly stub out 3rd party servers to be used against your automated development test cycle.

> Designed to feel familiar to use, without having restrictions forced upon you.

> Can be as simple as you want... Or as complex...

[![NuGet downloads](https://img.shields.io/badge/nuget-v0.1.3-blue.svg)](https://www.nuget.org/packages/StubServer)

Checkout the [Quickstart](https://github.com/agabani/StubServer/wiki/Quickstart) for more examples!

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