using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public class Setup<TRequest, TResponse> : IMultipleResponse<TResponse>
    {
        private readonly Func<TRequest, bool> _expression;

        private readonly Queue<List<Func<CancellationToken, Task<TResponse>>>> _responses =
            new Queue<List<Func<CancellationToken, Task<TResponse>>>>();

        private IEnumerable<Func<CancellationToken, Task<TResponse>>> _response;

        internal Setup(Expression<Func<TRequest, bool>> expression)
        {
            _expression = expression.Compile();
        }

        public IResponse<TResponse> Returns(Func<TResponse> response)
        {
            _responses.Enqueue(new List<Func<CancellationToken, Task<TResponse>>>
            {
                cancellationToken => Task.FromResult(response())
            });
            return this;
        }

        public IResponse<TResponse> Returns(Func<Task<TResponse>> response)
        {
            _responses.Enqueue(new List<Func<CancellationToken, Task<TResponse>>>
            {
                cancellationToken => response()
            });
            return this;
        }

        public IResponse<TResponse> Returns(Func<CancellationToken, Task<TResponse>> response)
        {
            _responses.Enqueue(new List<Func<CancellationToken, Task<TResponse>>>
            {
                response
            });
            return this;
        }

        public IMultipleResponse<TResponse> Then(Func<TResponse> response)
        {
            _responses.Last().Add(cancellationToken => Task.FromResult(response()));
            return this;
        }

        public IMultipleResponse<TResponse> Then(Func<Task<TResponse>> response)
        {
            _responses.Last().Add(cancellationToken => response());
            return this;
        }

        public IMultipleResponse<TResponse> Then(Func<CancellationToken, Task<TResponse>> response)
        {
            _responses.Last().Add(response);
            return this;
        }

        internal Task<TResponse> Result(TRequest request, CancellationToken cancellationToken)
        {
            return _expression(request)
                ? _responses.Any()
                    ? (_response = _responses.Dequeue()).First()(cancellationToken)
                    : _response.First()(cancellationToken)
                : Task.FromResult(default(TResponse));
        }

        internal IEnumerable<Task<TResponse>> Results(TRequest request, CancellationToken cancellationToken)
        {
            return _expression(request)
                ? _responses.Any()
                    ? (_response = _responses.Dequeue()).Select(func => func(cancellationToken))
                    : _response.Select(func => func(cancellationToken))
                : default(List<Task<TResponse>>);
        }
    }
}