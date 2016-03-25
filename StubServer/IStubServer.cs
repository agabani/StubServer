using System;
using System.Linq.Expressions;

namespace StubServer
{
    public interface IStubServer<TRequest, in TResponse> : IDisposable
    {
        ISetup<TResponse> Setup(Expression<Func<TRequest, bool>> expression);
    }
}