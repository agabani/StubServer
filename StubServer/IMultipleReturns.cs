using System;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public interface IMultipleReturns<TResponse>
    {
        IMultipleReturns<TResponse> Returns(Func<TResponse> response);
        IMultipleReturns<TResponse> Returns(Func<Task<TResponse>> response);
        IMultipleReturns<TResponse> Returns(Func<CancellationToken, Task<TResponse>> response);
        IMultipleReturns<TResponse> Then(Func<TResponse> response);
        IMultipleReturns<TResponse> Then(Func<Task<TResponse>> response);
        IMultipleReturns<TResponse> Then(Func<CancellationToken, Task<TResponse>> response);
    }
}