using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http.SelfHost;
using StubServer.Framework;

namespace StubServer.Http
{
    public partial class HttpStubServer : IDisposable
    {
        private HttpHandler _httpHandler;
        private HttpSelfHostServer _httpSelfHostServer;

        public HttpStubServer(Uri baseAddress)
        {
            _httpSelfHostServer = new HttpSelfHostServer(new HttpSelfHostConfiguration(baseAddress)
            {
                HostNameComparisonMode = HostNameComparisonMode.Exact
            }, _httpHandler = new HttpHandler());

            _httpSelfHostServer.OpenAsync().Wait();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public SingleReturn<HttpRequestMessage, HttpResponseMessage> When(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            return _httpHandler.AddSetup(expression);
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

    public partial class HttpStubServer
    {
        [Obsolete(Literals.SetupIsDeprecatedPleaseUseWhenInstead)]
        public SingleReturn<HttpRequestMessage, HttpResponseMessage> Setup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            return When(expression);
        }
    }
}