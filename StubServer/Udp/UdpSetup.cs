using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer.Udp
{
    public class UdpSetup : ISetup<byte[]>
    {
        private readonly Func<byte[], bool> _expression;
        private Func<CancellationToken, Task<byte[]>> _response;

        public UdpSetup(Expression<Func<byte[], bool>> expression)
        {
            _expression = expression.Compile();
        }

        public ISetup<byte[]> Returns(Func<byte[]> response)
        {
            _response = cancellationToken => Task.FromResult(response());
            return this;
        }

        public ISetup<byte[]> Returns(Func<Task<byte[]>> response)
        {
            _response = cancellationToken => response();
            return this;
        }

        public ISetup<byte[]> Returns(Func<CancellationToken, Task<byte[]>> response)
        {
            _response = response;
            return this;
        }

        internal Task<byte[]> Result(byte[] request, CancellationToken cancellationToken)
        {
            if (_expression(request))
            {
                return _response(cancellationToken);
            }

            return Task.FromResult<byte[]>(null);
        }
    }
}