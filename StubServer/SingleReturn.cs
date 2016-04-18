using System;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public partial class SingleReturn<TRequest, TResponse>
    {
        private readonly Setup<TRequest, TResponse> _setup;

        internal Task<TResponse> Result(TRequest request, CancellationToken cancellationToken)
        {
            return _setup.Result(request, cancellationToken);
        }
    }

    public partial class SingleReturn<TRequest, TResponse>
    {
        public SingleReturn(Setup<TRequest, TResponse> setup)
        {
            _setup = setup;
        }

        public SingleReturn<TRequest, TResponse> Return(Func<TResponse> response)
        {
            _setup.Return(response);
            return this;
        }

        public SingleReturn<TRequest, TResponse> Return(Func<Task<TResponse>> response)
        {
            _setup.Return(response);
            return this;
        }

        public SingleReturn<TRequest, TResponse> Return(Func<CancellationToken, Task<TResponse>> response)
        {
            _setup.Return(response);
            return this;
        }
    }

    public partial class SingleReturn<TRequest, TResponse>
    {
        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        public SingleReturn<TRequest, TResponse> Returns(Func<TResponse> response)
        {
            return Return(response);
        }

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        public SingleReturn<TRequest, TResponse> Returns(Func<Task<TResponse>> response)
        {
            return Return(response);
        }

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        public SingleReturn<TRequest, TResponse> Returns(Func<CancellationToken, Task<TResponse>> response)
        {
            return Return(response);
        }
    }
}