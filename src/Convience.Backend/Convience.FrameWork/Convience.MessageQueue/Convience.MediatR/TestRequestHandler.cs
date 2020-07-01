using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.MediatRs
{
    public class TestRequestHandler : IRequestHandler<TestRequest, string>
    {
        // handler中可以使用依赖注入
        public async Task<string> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(3000);
            Console.WriteLine(request.Message);
            return "ok";
        }
    }
}
