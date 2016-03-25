using System;
using System.Threading.Tasks;

namespace StubServer
{
    public interface ISetup<TResponse>
    {
        void Returns(Func<TResponse> response);
        void Returns(Func<Task<TResponse>> response);
    }
}