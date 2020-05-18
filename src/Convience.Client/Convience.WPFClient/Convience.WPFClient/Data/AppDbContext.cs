using Convience.WPFClient.Data.Entity;

using Microsoft.EntityFrameworkCore;

namespace Convience.WPFClient.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<LoginInfo> LoginInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
