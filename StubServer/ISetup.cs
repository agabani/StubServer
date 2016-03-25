using System;
using System.Net.Http;

namespace StubServer
{
    public interface ISetup
    {
        void Returns(Func<HttpResponseMessage> response);
    }
}