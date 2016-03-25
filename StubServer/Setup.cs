using System;
using System.Linq.Expressions;
using System.Net.Http;

namespace StubServer
{
    public class Setup : ISetup
    {
        private Func<HttpResponseMessage> _response;
        private readonly Func<HttpRequestMessage, bool> _expression;

        public Setup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            _expression = expression.Compile();
        }

        public void Returns(Func<HttpResponseMessage> response)
        {
            _response = response;
        }

        public HttpResponseMessage Result(HttpRequestMessage request)
        {
            return _expression(request) ? _response() : null;
        }
    }
}