using backend.data.Infrastructure;

using Convience.Entity.Data;

namespace Convience.Entity.Entity
{

    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class MenuTree
    {
        public int Id { get; set; }

        public int Ancestor { get; set; }

        public int Descendant { get; set; }

        public int Length { get; set; }
    }
}
