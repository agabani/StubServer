using System;
using System.Linq.Expressions;
using System.Net.Http;

namespace StubServer
{
    public interface IHttpMockServer : IDisposable
    {
        ISetup Setup(Expression<Func<HttpRequestMessage, bool>> expression);
    }
}