using System;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public partial interface IMultipleReturns<TResponse>
    {
        IMultipleReturns<TResponse> Return(Func<TResponse> response);
        IMultipleReturns<TResponse> Return(Func<Task<TResponse>> response);
        IMultipleReturns<TResponse> Return(Func<CancellationToken, Task<TResponse>> response);
        IMultipleReturns<TResponse> Then(Func<TResponse> response);
        IMultipleReturns<TResponse> Then(Func<Task<TResponse>> response);
        IMultipleReturns<TResponse> Then(Func<CancellationToken, Task<TResponse>> response);
    }

    public partial interface IMultipleReturns<TResponse>
    {
        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        IMultipleReturns<TResponse> Returns(Func<TResponse> response);

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        IMultipleReturns<TResponse> Returns(Func<Task<TResponse>> response);

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        IMultipleReturns<TResponse> Returns(Func<CancellationToken, Task<TResponse>> response);
    }
}