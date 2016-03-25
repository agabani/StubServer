using System;
using System.Net.Http;
using NUnit.Framework;
using StubServer.Http;

namespace StubServer.Tests.Acceptance.Http
{
    internal abstract class HttpStubServerTests : IDisposable
    {
        protected HttpClient HttpClient;
        protected HttpRequestMessage HttpRequestMessage;
        protected HttpResponseMessage HttpResponseMessage;
        protected IHttpStubServer HttpStubServer;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SetUp]
        public void SetUp()
        {
            var baseAddress = new Uri("http://localhost:5050");

            HttpStubServer = new HttpStubServer(baseAddress);

            HttpClient = new HttpClient
            {
                BaseAddress = baseAddress,
                Timeout = TimeSpan.FromSeconds(1)
            };
        }

        [TearDown]
        public void TearDown()
        {
            HttpRequestMessage.Dispose();
            HttpResponseMessage.Dispose();
            HttpClient.Dispose();
            HttpStubServer.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (HttpRequestMessage != null)
                {
                    HttpRequestMessage.Dispose();
                    HttpRequestMessage = null;
                }

                if (HttpResponseMessage != null)
                {
                    HttpResponseMessage.Dispose();
                    HttpResponseMessage = null;
                }

                if (HttpClient != null)
                {
                    HttpClient.Dispose();
                    HttpClient = null;
                }

                if (HttpStubServer != null)
                {
                    HttpStubServer.Dispose();
                    HttpStubServer = null;
                }
            }
        }
    }
}