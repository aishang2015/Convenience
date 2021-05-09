using Convience.Background;
using Convience.Entity.Data;
using Convience.Entity.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure
{
    public class InitDataJob : AbstractBackgroundJob
    {
        private UserManager<SystemUser> _userManager;

        private SystemIdentityDbContext _systemIdentityDbContext;

        public InitDataJob(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<SystemUser>>();
            _systemIdentityDbContext = scope.ServiceProvider.GetRequiredService<SystemIdentityDbContext>();
            Hours = 6;
            Seconds = 0;
        }

        public override Task DoWork()
        {
            DbContextSeed.InitUsers(_userManager);
            DbContextSeed.InitMenuTree(_systemIdentityDbContext);
            return Task.CompletedTask;
        }
    }
}
