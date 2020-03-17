using backend.data.Infrastructure;
using backend.entity.backend.api;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.api.Infrastruct
{
    public class SystemIdentityDbContext : IdentityDbContext<SystemUser, SystemRole, string>
    {
        public SystemIdentityDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurationEntity(typeof(SystemIdentityDbContext));
        }

    }
}
