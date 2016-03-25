using System;
using System.Linq.Expressions;
using System.Net.Http;

namespace StubServer.Http
{
    internal class HttpSetup : ISetup<HttpResponseMessage>
    {
        private readonly Func<HttpRequestMessage, bool> _expression;
        private Func<HttpResponseMessage> _response;

        internal HttpSetup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            _expression = expression.Compile();
        }

        public void Returns(Func<HttpResponseMessage> response)
        {
            _response = response;
        }

        internal HttpResponseMessage Result(HttpRequestMessage request)
        {
            return _expression(request) ? _response() : null;
        }
    }
}