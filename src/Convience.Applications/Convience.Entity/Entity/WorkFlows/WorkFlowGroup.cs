using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity.WorkFlows
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class WorkFlowGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }
    }
}
