using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http.SelfHost;

namespace StubServer
{
    public class HttpMockServer : IHttpMockServer
    {
        private HttpSelfHostServer _httpSelfHostServer;
        private MockHttpMessageHandler _mockHttpMessageHandler;

        public HttpMockServer(Uri baseAddress)
        {
            _httpSelfHostServer = new HttpSelfHostServer(new HttpSelfHostConfiguration(baseAddress),
                _mockHttpMessageHandler = new MockHttpMessageHandler());

            _httpSelfHostServer.OpenAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ISetup Setup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            return _mockHttpMessageHandler.AddSetup(expression);
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

                if (_mockHttpMessageHandler != null)
                {
                    _mockHttpMessageHandler.Dispose();
                    _mockHttpMessageHandler = null;
                }
            }
        }
    }
}