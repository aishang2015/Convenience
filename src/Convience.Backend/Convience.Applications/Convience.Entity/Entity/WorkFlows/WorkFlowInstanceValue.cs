using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity.WorkFlows
{
    /// <summary>
    /// 工作流实例表单值
    /// </summary>
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class WorkFlowInstanceValue
    {
        public int Id { get; set; }

        public string FormControlDomId { get; set; }

        public string Value { get; set; }

        #region

        public int WorkFlowInstanceId { get; set; }

        public WorkFlowInstance WorkFlowInstance { get; set; }

        #endregion
    }
}
