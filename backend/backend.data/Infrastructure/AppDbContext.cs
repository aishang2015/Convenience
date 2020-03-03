using Microsoft.EntityFrameworkCore;

namespace backend.data.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurationEntity(typeof(AppDbContext));
        }
    }
}
