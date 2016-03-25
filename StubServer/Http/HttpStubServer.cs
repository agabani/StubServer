using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http.SelfHost;

namespace StubServer.Http
{
    public class HttpStubServer : IHttpStubServer
    {
        private HttpSelfHostServer _httpSelfHostServer;
        private StubHttpMessageHandler _stubHttpMessageHandler;

        public HttpStubServer(Uri baseAddress)
        {
            _httpSelfHostServer = new HttpSelfHostServer(new HttpSelfHostConfiguration(baseAddress),
                _stubHttpMessageHandler = new StubHttpMessageHandler());

            _httpSelfHostServer.OpenAsync();
        }

        public ISetup<HttpResponseMessage> Setup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            return _stubHttpMessageHandler.AddSetup(expression);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_httpSelfHostServer != null)
                {
                    _httpSelfHostServer.Dispose();
                    _httpSelfHostServer = null;
                }

                if (_stubHttpMessageHandler != null)
                {
                    _stubHttpMessageHandler.Dispose();
                    _stubHttpMessageHandler = null;
                }
            }
        }
    }
}