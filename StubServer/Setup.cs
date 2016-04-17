using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public partial class Setup<TRequest, TResponse>
    {
        private readonly Func<TRequest, bool> _expression;

        private readonly Queue<List<Func<CancellationToken, Task<TResponse>>>> _responses =
            new Queue<List<Func<CancellationToken, Task<TResponse>>>>();

        private IEnumerable<Func<CancellationToken, Task<TResponse>>> _response;

        internal Setup(Expression<Func<TRequest, bool>> expression)
        {
            _expression = expression.Compile();
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

    public partial class Setup<TRequest, TResponse> : ISingleReturns<TResponse>
    {
        public ISingleReturns<TResponse> Return(Func<TResponse> response)
        {
            _responses.Enqueue(new List<Func<CancellationToken, Task<TResponse>>>
            {
                cancellationToken => Task.FromResult(response())
            });
            return this;
        }

        public ISingleReturns<TResponse> Return(Func<Task<TResponse>> response)
        {
            _responses.Enqueue(new List<Func<CancellationToken, Task<TResponse>>>
            {
                cancellationToken => response()
            });
            return this;
        }

        public ISingleReturns<TResponse> Return(Func<CancellationToken, Task<TResponse>> response)
        {
            _responses.Enqueue(new List<Func<CancellationToken, Task<TResponse>>>
            {
                response
            });
            return this;
        }
    }

    public partial class Setup<TRequest, TResponse> : IMultipleReturns<TResponse>
    {
        IMultipleReturns<TResponse> IMultipleReturns<TResponse>.Return(Func<TResponse> response)
        {
            return (IMultipleReturns<TResponse>) Return(response);
        }

        IMultipleReturns<TResponse> IMultipleReturns<TResponse>.Return(Func<Task<TResponse>> response)
        {
            return (IMultipleReturns<TResponse>) Return(response);
        }

        IMultipleReturns<TResponse> IMultipleReturns<TResponse>.Return(
            Func<CancellationToken, Task<TResponse>> response)
        {
            return (IMultipleReturns<TResponse>) Return(response);
        }

        public IMultipleReturns<TResponse> Then(Func<TResponse> response)
        {
            if (!_responses.Any())
            {
                throw new InvalidOperationException(Literals.NoReturnsHaveBeenConfigured);
            }

            _responses.Last().Add(cancellationToken => Task.FromResult(response()));
            return this;
        }

        public IMultipleReturns<TResponse> Then(Func<Task<TResponse>> response)
        {
            if (!_responses.Any())
            {
                throw new InvalidOperationException(Literals.NoReturnsHaveBeenConfigured);
            }

            _responses.Last().Add(cancellationToken => response());
            return this;
        }

        public IMultipleReturns<TResponse> Then(Func<CancellationToken, Task<TResponse>> response)
        {
            if (!_responses.Any())
            {
                throw new InvalidOperationException(Literals.NoReturnsHaveBeenConfigured);
            }

            _responses.Last().Add(response);
            return this;
        }
    }

    public partial class Setup<TRequest, TResponse>
    {
        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        public ISingleReturns<TResponse> Returns(Func<TResponse> response)
        {
            return Return(response);
        }

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        public ISingleReturns<TResponse> Returns(Func<Task<TResponse>> response)
        {
            return Return(response);
        }

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        public ISingleReturns<TResponse> Returns(Func<CancellationToken, Task<TResponse>> response)
        {
            return Return(response);
        }

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        IMultipleReturns<TResponse> IMultipleReturns<TResponse>.Returns(Func<TResponse> response)
        {
            return (IMultipleReturns<TResponse>) Return(response);
        }

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        IMultipleReturns<TResponse> IMultipleReturns<TResponse>.Returns(Func<Task<TResponse>> response)
        {
            return (IMultipleReturns<TResponse>) Return(response);
        }

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        IMultipleReturns<TResponse> IMultipleReturns<TResponse>.Returns(
            Func<CancellationToken, Task<TResponse>> response)
        {
            return (IMultipleReturns<TResponse>) Return(response);
        }
    }
}