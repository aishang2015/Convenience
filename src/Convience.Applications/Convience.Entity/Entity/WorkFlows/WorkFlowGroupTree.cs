using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity.WorkFlows
{

    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class WorkFlowGroupTree
    {
        public int Id { get; set; }

        public int Ancestor { get; set; }

        public int Descendant { get; set; }

        public int Length { get; set; }
    }
}
