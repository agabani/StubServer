using System;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public interface IResponse<TResponse>
    {
        IResponse<TResponse> Returns(Func<TResponse> response);
        IResponse<TResponse> Returns(Func<Task<TResponse>> response);
        IResponse<TResponse> Returns(Func<CancellationToken, Task<TResponse>> response);
    }
}