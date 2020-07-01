using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class Position
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }
    }
}
