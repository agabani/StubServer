using System.Net.Http;

namespace StubServer.Http
{
    public interface IHttpStubServer : IStubServer<HttpRequestMessage, HttpResponseMessage>
    {
    }
}