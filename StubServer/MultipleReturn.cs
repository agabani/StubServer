using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public partial class MultipleReturn<TRequest, TResponse>
    {
        private readonly Setup<TRequest, TResponse> _setup;

        public MultipleReturn(Setup<TRequest, TResponse> setup)
        {
            _setup = setup;
        }

        internal IEnumerable<Task<TResponse>> Results(TRequest request, CancellationToken cancellationToken)
        {
            return _setup.Results(request, cancellationToken);
        }
    }

    public partial class MultipleReturn<TRequest, TResponse>
    {
        public MultipleReturn<TRequest, TResponse> Return(Func<TResponse> response)
        {
            _setup.Return(response);
            return this;
        }

        public MultipleReturn<TRequest, TResponse> Return(Func<Task<TResponse>> response)
        {
            _setup.Return(response);
            return this;
        }

        public MultipleReturn<TRequest, TResponse> Return(Func<CancellationToken, Task<TResponse>> response)
        {
            _setup.Return(response);
            return this;
        }

        public MultipleReturn<TRequest, TResponse> Then(Func<TResponse> response)
        {
            _setup.Then(response);
            return this;
        }

        public MultipleReturn<TRequest, TResponse> Then(Func<Task<TResponse>> response)
        {
            _setup.Then(response);
            return this;
        }

        public MultipleReturn<TRequest, TResponse> Then(Func<CancellationToken, Task<TResponse>> response)
        {
            _setup.Then(response);
            return this;
        }
    }

    public partial class MultipleReturn<TRequest, TResponse>
    {
        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        public MultipleReturn<TRequest, TResponse> Returns(Func<TResponse> response)
        {
            return Return(response);
        }

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        public MultipleReturn<TRequest, TResponse> Returns(Func<Task<TResponse>> response)
        {
            return Return(response);
        }

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        public MultipleReturn<TRequest, TResponse> Returns(Func<CancellationToken, Task<TResponse>> response)
        {
            return Return(response);
        }
    }
}