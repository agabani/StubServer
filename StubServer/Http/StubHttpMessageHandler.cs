using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer.Http
{
    internal class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly List<HttpSetup> _setups = new List<HttpSetup>();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            foreach (var setup in _setups)
            {
                var httpResponseMessage = await setup.Result(request);

                if (httpResponseMessage != null)
                {
                    return httpResponseMessage;
                }
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        internal ISetup<HttpResponseMessage> AddSetup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            HttpSetup httpSetup;
            _setups.Add(httpSetup = new HttpSetup(expression));
            return httpSetup;
        }
    }
}