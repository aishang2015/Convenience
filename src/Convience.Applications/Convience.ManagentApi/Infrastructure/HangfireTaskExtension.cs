using Convience.Entity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
