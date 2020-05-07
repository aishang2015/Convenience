using Hangfire;

using Microsoft.Extensions.DependencyInjection;

namespace Convience.Hangfire
{
    public class HangFireJobActivator : JobActivator
    {
        readonly IServiceScopeFactory _serviceScopeFactory;
        public HangFireJobActivator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new HangfireJobActivatorScope(_serviceScopeFactory.CreateScope());
        }
    }
}
