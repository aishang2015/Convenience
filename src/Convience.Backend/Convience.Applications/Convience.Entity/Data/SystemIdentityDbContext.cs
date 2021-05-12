using Convience.Entity.Entity.Identity;
using Convience.EntityFrameWork.Infrastructure;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Convience.Entity.Data
{
    public class SystemIdentityDbContext :
        IdentityDbContext<SystemUser, SystemRole, int, SystemUserClaim, SystemUserRole, SystemUserLogin, SystemRoleClaim, SystemUserToken>
    {
        public SystemIdentityDbContext(DbContextOptions<SystemIdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurationEntity(typeof(SystemIdentityDbContext));
        }

    }
}
