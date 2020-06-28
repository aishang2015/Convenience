using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

using System;

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

        // 节点名称
        public string NodeName { get; set; }

        // 处置人姓名
        public string HandlePeopleName { get; set; }

        // 处理人账号
        public string HandlePepleAccount { get; set; }

        // 处置评论
        public string HandleComment { get; set; }

        // 处理情况
        public HandleStateEnum HandleState { get; set; }

        // 处理时间
        public DateTime HandleTime { get; set; }

        #region

        public int WorkFlowInstanceId { get; set; }

        public WorkFlowInstance WorkFlowInstance { get; set; }

        #endregion
    }

    public enum HandleStateEnum
    {
        未处理 = 1,
        通过 = 2,
        拒绝 = 3
    }
}
