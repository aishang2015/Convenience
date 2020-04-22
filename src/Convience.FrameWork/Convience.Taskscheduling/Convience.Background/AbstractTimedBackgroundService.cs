using Microsoft.Extensions.Hosting;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.Background
{
    public abstract class AbstractTimedBackgroundService : BackgroundService
    {
        private readonly AbstractBackgroundJob _backgroundJob;

        private readonly TimeSpan _timeSpan;

        public AbstractTimedBackgroundService(AbstractBackgroundJob backgroundJob)
        {
            _backgroundJob = backgroundJob;
            _timeSpan = GetDelaySpan();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_timeSpan, stoppingToken);
                await _backgroundJob.DoWork();
            }
        }

        private TimeSpan GetDelaySpan()
        {
            return TimeSpan.FromDays(_backgroundJob.Days)
                .Add(TimeSpan.FromHours(_backgroundJob.Hours))
                .Add(TimeSpan.FromMinutes(_backgroundJob.Minutes))
                .Add(TimeSpan.FromSeconds(_backgroundJob.Seconds));
        }
    }
}
