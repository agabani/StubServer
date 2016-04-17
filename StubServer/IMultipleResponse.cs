using System;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer
{
    public interface IMultipleResponse<TResponse> : IResponse<TResponse>
    {
        IMultipleResponse<TResponse> Then(Func<TResponse> response);
        IMultipleResponse<TResponse> Then(Func<Task<TResponse>> response);
        IMultipleResponse<TResponse> Then(Func<CancellationToken, Task<TResponse>> response);
    }
}