using System;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public partial interface ISingleReturn<TResponse>
    {
        ISingleReturn<TResponse> Return(Func<TResponse> response);
        ISingleReturn<TResponse> Return(Func<Task<TResponse>> response);
        ISingleReturn<TResponse> Return(Func<CancellationToken, Task<TResponse>> response);
    }

    public partial interface ISingleReturn<TResponse>
    {
        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        ISingleReturn<TResponse> Returns(Func<TResponse> response);

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        ISingleReturn<TResponse> Returns(Func<Task<TResponse>> response);

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        ISingleReturn<TResponse> Returns(Func<CancellationToken, Task<TResponse>> response);
    }
}