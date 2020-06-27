using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

using System;
using System.Collections.Generic;

namespace Convience.Entity.Entity.WorkFlows
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class WorkFlowInstance
    {
        public int Id { get; set; }

        public string WorkFlowName { get; set; }

        public WorkFlowInstanceStateEnum WorkFlowInstanceState { get; set; }

        // 当前节点id
        public int CurrentNodeId { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatedUserAccount { get; set; }

        public string CreatedUserName { get; set; }


        #region

        public int WorkFlowId { get; set; }

        public WorkFlow WorkFlow { get; set; }

        public List<WorkFlowInstanceRoute> WorkFlowInstanceRoutes { get; set; }

        public List<WorkFlowInstanceValue> WorkFlowInstanceValues { get; set; }

        #endregion
    }

    public enum WorkFlowInstanceStateEnum
    {
        NoCommitted = 1,    // 未提交
        CirCulation = 2,    // 流转
        ReturnBack = 3,     // 打回
        End = 4,            // 结束
        BadEnd = 5,         // 无法进行
        Cancel = 6          // 取消
    }
}
