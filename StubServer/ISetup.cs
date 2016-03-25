using System;

namespace StubServer
{
    public interface ISetup<in TResponse>
    {
        void Returns(Func<TResponse> response);
    }
}