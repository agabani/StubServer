using System;
using System.Net.Http;
using StubServer.Http;

namespace StubServer.Tests.Acceptance.Http
{
    internal abstract class HttpStubServerTests
    {
        protected readonly Uri BaseAddress = new Uri("http://localhost:5050");

        protected HttpClient NewHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = BaseAddress,
                Timeout = TimeSpan.FromSeconds(1)
            };
        }

        protected IHttpStubServer NewStubServer()
        {
            return new HttpStubServer(BaseAddress);
        }

        protected void Cleanup(IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}