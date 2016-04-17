using System;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public partial interface IMultipleReturn<TResponse>
    {
        IMultipleReturn<TResponse> Return(Func<TResponse> response);
        IMultipleReturn<TResponse> Return(Func<Task<TResponse>> response);
        IMultipleReturn<TResponse> Return(Func<CancellationToken, Task<TResponse>> response);
        IMultipleReturn<TResponse> Then(Func<TResponse> response);
        IMultipleReturn<TResponse> Then(Func<Task<TResponse>> response);
        IMultipleReturn<TResponse> Then(Func<CancellationToken, Task<TResponse>> response);
    }

    public partial interface IMultipleReturn<TResponse>
    {
        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        IMultipleReturn<TResponse> Returns(Func<TResponse> response);

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        IMultipleReturn<TResponse> Returns(Func<Task<TResponse>> response);

        [Obsolete(Literals.ReturnsIsDeprecatedPleaseUseReturnInstead)]
        IMultipleReturn<TResponse> Returns(Func<CancellationToken, Task<TResponse>> response);
    }
}