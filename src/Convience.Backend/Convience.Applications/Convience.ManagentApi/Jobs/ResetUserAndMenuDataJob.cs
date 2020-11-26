using Convience.Entity.Data;
using Convience.ManagentApi.Infrastructure;

using Microsoft.AspNetCore.Identity;

namespace Convience.ManagentApi.Jobs
{
    public class ResetUserAndMenuDataJob
    {
        private readonly SystemIdentityDbContext _systemIdentityDbContext;

        private readonly UserManager<SystemUser> _userManager;

        public ResetUserAndMenuDataJob(SystemIdentityDbContext systemIdentityDbContext,
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
