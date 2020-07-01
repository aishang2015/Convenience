using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity.WorkFlows
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class WorkFlowLink
    {
        public int Id { get; set; }

        public string SourceId { get; set; }

        public string TargetId { get; set; }

        public string Connection { get; set; }

        #region

        public int WorkFlowId { get; set; }

        public WorkFlow WorkFlow { get; set; }

        #endregion
    }
}
