using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StubServer.Udp
{
    public class UdpSetup : ISetup<byte[]>
    {
        private readonly Func<byte[], bool> _expression;

        private readonly Queue<Func<CancellationToken, Task<byte[]>>> _responses =
            new Queue<Func<CancellationToken, Task<byte[]>>>();

        private Func<CancellationToken, Task<byte[]>> _response;

        public UdpSetup(Expression<Func<byte[], bool>> expression)
        {
            _expression = expression.Compile();
        }

        public ISetup<byte[]> Returns(Func<byte[]> response)
        {
            _responses.Enqueue(cancellationToken => Task.FromResult(response()));
            return this;
        }

        public ISetup<byte[]> Returns(Func<Task<byte[]>> response)
        {
            _responses.Enqueue(cancellationToken => response());
            return this;
        }

        public ISetup<byte[]> Returns(Func<CancellationToken, Task<byte[]>> response)
        {
            _responses.Enqueue(response);
            return this;
        }

        internal Task<byte[]> Result(byte[] request, CancellationToken cancellationToken)
        {
            return _expression(request)
                ? _responses.Any()
                    ? (_response = _responses.Dequeue())(cancellationToken)
                    : _response(cancellationToken)
                : Task.FromResult<byte[]>(null);
        }
    }
}