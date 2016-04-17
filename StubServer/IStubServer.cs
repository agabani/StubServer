using System;
using System.Linq.Expressions;

namespace StubServer
{
    public interface IStubServer<TRequest, TResponse> : IDisposable
    {
        IResponse<TResponse> Setup(Expression<Func<TRequest, bool>> expression);
    }
}