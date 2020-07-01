using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.MediatRs
{
    public class TestEventhandler : INotificationHandler<TestEvent>
    {
        // handler中可以使用依赖注入
        public async Task Handle(TestEvent notification, CancellationToken cancellationToken)
        {
            await Task.Delay(3000);
            Console.WriteLine(notification.Message);
        }
    }
}
