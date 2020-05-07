using Convience.Entity.Data;

using Microsoft.AspNetCore.Identity;

namespace Convience.ManagentApi.Infrastructure
{
    public class HangfireResetDataJob
    {
        private readonly SystemIdentityDbContext _systemIdentityDbContext;

        private readonly UserManager<SystemUser> _userManager;

        public HangfireResetDataJob(SystemIdentityDbContext systemIdentityDbContext,
            UserManager<SystemUser> userManager)
        {
            _systemIdentityDbContext = systemIdentityDbContext;
            _userManager = userManager;
        }

        public void Run()
        {
            DbContextSeed.InitUsers(_userManager);
            DbContextSeed.InitMenuTree(_systemIdentityDbContext);
        }
    }
}
