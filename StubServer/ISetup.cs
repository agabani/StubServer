using System;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public interface ISetup<TResponse>
    {
        ISetup<TResponse> Returns(Func<TResponse> response);
        ISetup<TResponse> Returns(Func<Task<TResponse>> response);
        ISetup<TResponse> Returns(Func<CancellationToken, Task<TResponse>> response);
    }
}