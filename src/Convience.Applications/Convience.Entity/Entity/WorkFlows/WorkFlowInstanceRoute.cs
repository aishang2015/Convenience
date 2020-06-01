using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity.WorkFlows
{
    /// <summary>
    /// 工作流实例流转信息
    /// </summary>
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class WorkFlowInstanceRoute
    {
        public int Id { get; set; }

        // 节点id
        public int NodeId { get; set; }

        // 处置人
        public string HandlePeople { get; set; }

        // 处置情况
        public string HandleInfo { get; set; }

        #region

        public int WorkFlowInstanceId { get; set; }

        public WorkFlowInstance WorkFlowInstance { get; set; }

        #endregion
    }
}
