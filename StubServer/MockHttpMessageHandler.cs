using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly List<Setup> _setups = new List<Setup>();

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            foreach (var setup in _setups)
            {
                var httpResponseMessage = setup.Result(request);

                if (httpResponseMessage != null)
                {
                    return Task.FromResult(httpResponseMessage);
                }
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        public ISetup AddSetup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            Setup setup;
            _setups.Add(setup = new Setup(expression));
            return setup;
        }
    }
}