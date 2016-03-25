using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace StubServer.Http
{
    internal class HttpSetup : ISetup<HttpResponseMessage>
    {
        private readonly Func<HttpRequestMessage, bool> _expression;
        private Func<Task<HttpResponseMessage>> _response;

        internal HttpSetup(Expression<Func<HttpRequestMessage, bool>> expression)
        {
            _expression = expression.Compile();
        }

        public void Returns(Func<HttpResponseMessage> response)
        {
            _response = () => Task.FromResult(response());
        }

        public void Returns(Func<Task<HttpResponseMessage>> response)
        {
            _response = response;
        }

        internal Task<HttpResponseMessage> Result(HttpRequestMessage request)
        {
            return _expression(request) ? _response() : Task.FromResult<HttpResponseMessage>(null);
        }
    }
}