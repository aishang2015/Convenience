using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure.BackgroudTask
{
    public class EventListenerBackgroundService : BackgroundService
    {
        private readonly SimpleEventListener _listener;

        public EventListenerBackgroundService(SimpleEventListener listener)
        {
            _listener = listener;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }


}
