using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer.Http
{
    internal class HttpHandler : HttpMessageHandler
    {
        private readonly List<Setup<HttpRequestMessage, HttpResponseMessage>> _setups = new List<Setup<HttpRequestMessage, HttpResponseMessage>>();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            foreach (var setup in _setups)
            {
                var httpResponseMessage = await setup
                    .Result(request, cancellationToken)
                    .ConfigureAwait(false);

                if (httpResponseMessage != null)
                {
                    return httpResponseMessage;
                }
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        internal ISingleReturn<HttpResponseMessage> AddSetup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            Setup<HttpRequestMessage, HttpResponseMessage> setup;
            _setups.Add(setup = new Setup<HttpRequestMessage, HttpResponseMessage>(expression));
            return setup;
        }
    }
}