
using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity
{

    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class MenuTree
    {
        public int Id { get; set; }

        public int Ancestor { get; set; }

        public int Descendant { get; set; }

        public int Length { get; set; }

        public MenuTree() { }

        public MenuTree(int id, int ancestor, int descendant, int length)
        {
            Id = id;
            Ancestor = ancestor;
            Descendant = descendant;
            Length = length;
        }
    }
}
