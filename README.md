# StubServer

> Quickly stub out 3rd party servers to be used against your automated development test cycle.

> Designed to feel familiar to use, without having restrictions forced upon you.

> Can be as simple as you want... Or as complex...

[![NuGet downloads](https://img.shields.io/badge/nuget-v0.1.2-blue.svg)](https://www.nuget.org/packages/StubServer)

### Example Http StubServer
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

Checkout the [Quickstart](https://github.com/agabani/StubServer/wiki/Quickstart) for more examples!