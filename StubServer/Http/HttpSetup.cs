using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer.Http
{
    internal class HttpSetup : ISetup<HttpResponseMessage>
    {
        private readonly Func<HttpRequestMessage, bool> _expression;
        private Func<CancellationToken, Task<HttpResponseMessage>> _response;

        internal HttpSetup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            _expression = expression.Compile();
        }

        public void Returns(Func<HttpResponseMessage> response)
        {
            _response = cancellationToken => Task.FromResult(response());
        }

        public void Returns(Func<Task<HttpResponseMessage>> response)
        {
            _response = cancellationToken => response();
        }

        public void Returns(Func<CancellationToken, Task<HttpResponseMessage>> response)
        {
            _response = response;
        }

        internal Task<HttpResponseMessage> Result(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _expression(request) ? _response(cancellationToken) : Task.FromResult<HttpResponseMessage>(null);
        }
    }
}