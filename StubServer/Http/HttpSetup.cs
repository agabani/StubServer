using System;
using System.Linq.Expressions;
using System.Net.Http;

namespace StubServer.Http
{
    public class HttpSetup : ISetup<HttpResponseMessage>
    {
        private readonly Func<HttpRequestMessage, bool> _expression;
        private Func<HttpResponseMessage> _response;

        public HttpSetup(Expression<Func<HttpRequestMessage, bool>> expression)
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