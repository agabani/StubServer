using System;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public interface ISingleReturns<TResponse>
    {
        ISingleReturns<TResponse> Returns(Func<TResponse> response);
        ISingleReturns<TResponse> Returns(Func<Task<TResponse>> response);
        ISingleReturns<TResponse> Returns(Func<CancellationToken, Task<TResponse>> response);
    }
}