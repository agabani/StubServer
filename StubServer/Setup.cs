using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public class Setup<TRequest, TResponse> : ISetup<TResponse>
    {
        private readonly Func<TRequest, bool> _expression;

        private readonly Queue<Func<CancellationToken, Task<TResponse>>> _responses =
            new Queue<Func<CancellationToken, Task<TResponse>>>();

        private Func<CancellationToken, Task<TResponse>> _response;

        internal Setup(Expression<Func<TRequest, bool>> expression)
        {
            _expression = expression.Compile();
        }

        public ISetup<TResponse> Returns(Func<TResponse> response)
        {
            _responses.Enqueue(cancellationToken => Task.FromResult(response()));
            return this;
        }

        public ISetup<TResponse> Returns(Func<Task<TResponse>> response)
        {
            _responses.Enqueue(cancellationToken => response());
            return this;
        }

        public ISetup<TResponse> Returns(Func<CancellationToken, Task<TResponse>> response)
        {
            _responses.Enqueue(response);
            return this;
        }

        internal Task<TResponse> Result(TRequest request, CancellationToken cancellationToken)
        {
            return _expression(request)
                ? _responses.Any()
                    ? (_response = _responses.Dequeue())(cancellationToken)
                    : _response(cancellationToken)
                : Task.FromResult(default(TResponse));
        }
    }
}