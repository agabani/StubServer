using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer.Http
{
    internal class HttpSetup : ISetup<HttpResponseMessage>
    {
        private readonly Func<HttpRequestMessage, bool> _expression;

        private readonly Queue<Func<CancellationToken, Task<HttpResponseMessage>>> _responses =
            new Queue<Func<CancellationToken, Task<HttpResponseMessage>>>();

        private Func<CancellationToken, Task<HttpResponseMessage>> _response;

        internal HttpSetup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            _expression = expression.Compile();
        }

        public ISetup<HttpResponseMessage> Returns(Func<HttpResponseMessage> response)
        {
            _responses.Enqueue(cancellationToken => Task.FromResult(response()));
            return this;
        }

        public ISetup<HttpResponseMessage> Returns(Func<Task<HttpResponseMessage>> response)
        {
            _responses.Enqueue(cancellationToken => response());
            return this;
        }

        public ISetup<HttpResponseMessage> Returns(Func<CancellationToken, Task<HttpResponseMessage>> response)
        {
            _responses.Enqueue(response);
            return this;
        }

        internal Task<HttpResponseMessage> Result(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _expression(request)
                ? _responses.Any()
                    ? (_response = _responses.Dequeue())(cancellationToken)
                    : _response(cancellationToken)
                : Task.FromResult<HttpResponseMessage>(null);
        }
    }
}