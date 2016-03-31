using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer.Http
{
    internal class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly List<Setup<HttpRequestMessage, HttpResponseMessage>> _setups = new List<Setup<HttpRequestMessage, HttpResponseMessage>>();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            foreach (var setup in _setups)
            {
                var httpResponseMessage = await setup.Result(request, cancellationToken);

                if (httpResponseMessage != null)
                {
                    return httpResponseMessage;
                }
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        internal ISetup<HttpResponseMessage> AddSetup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            Setup<HttpRequestMessage, HttpResponseMessage> httpSetup;
            _setups.Add(httpSetup = new Setup<HttpRequestMessage, HttpResponseMessage>(expression));
            return httpSetup;
        }
    }
}