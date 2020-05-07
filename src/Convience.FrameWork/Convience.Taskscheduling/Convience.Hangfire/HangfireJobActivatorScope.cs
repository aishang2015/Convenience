using Hangfire;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace Convience.Hangfire
{
    public class HangfireJobActivatorScope : JobActivatorScope
    {
        IServiceScope _serviceScope;
        public HangfireJobActivatorScope(IServiceScope serviceScope)
        {
            if (serviceScope == null) throw new ArgumentNullException(nameof(serviceScope));
            _serviceScope = serviceScope;
        }
        public override object Resolve(Type type)
        {
            return _serviceScope.ServiceProvider.GetService(type);
        }
        public override void DisposeScope()
        {
            _serviceScope.Dispose();
        }
    }
}
