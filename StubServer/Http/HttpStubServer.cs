using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http.SelfHost;

namespace StubServer.Http
{
    public class HttpStubServer : IDisposable
    {
        private HttpSelfHostServer _httpSelfHostServer;
        private HttpHandler _httpHandler;

        public HttpStubServer(Uri baseAddress)
        {
            _httpSelfHostServer = new HttpSelfHostServer(new HttpSelfHostConfiguration(baseAddress)
            {
                HostNameComparisonMode = HostNameComparisonMode.Exact
            }, _httpHandler = new HttpHandler());

            _httpSelfHostServer.OpenAsync().Wait();
        }

        public ISingleReturns<HttpResponseMessage> Setup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            return _httpHandler.AddSetup(expression);
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
                    _httpSelfHostServer.CloseAsync().Wait();
                    _httpSelfHostServer.Dispose();
                    _httpSelfHostServer = null;
                }

                if (_httpHandler != null)
                {
                    _httpHandler.Dispose();
                    _httpHandler = null;
                }
            }
        }
    }
}