using System;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public partial interface ISingleReturns<TResponse>
    {
        ISingleReturns<TResponse> Return(Func<TResponse> response);
        ISingleReturns<TResponse> Return(Func<Task<TResponse>> response);
        ISingleReturns<TResponse> Return(Func<CancellationToken, Task<TResponse>> response);
    }

    public partial interface ISingleReturns<TResponse>
    {
        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        ISingleReturns<TResponse> Returns(Func<TResponse> response);

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        ISingleReturns<TResponse> Returns(Func<Task<TResponse>> response);

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        ISingleReturns<TResponse> Returns(Func<CancellationToken, Task<TResponse>> response);
    }
}