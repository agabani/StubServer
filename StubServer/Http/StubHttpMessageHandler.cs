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
    public class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly List<HttpSetup> _setups = new List<HttpSetup>();

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            foreach (var httpResponseMessage in _setups
                .Select(setup => setup.Result(request))
                .Where(httpResponseMessage => httpResponseMessage != null))
            {
                return Task.FromResult(httpResponseMessage);
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        public ISetup<HttpResponseMessage> AddSetup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            HttpSetup httpSetup;
            _setups.Add(httpSetup = new HttpSetup(expression));
            return httpSetup;
        }
    }
}