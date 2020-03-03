using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.data.Infrastructure
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppIdentityDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurationEntity(typeof(AppIdentityDbContext));
        }

    }
}
