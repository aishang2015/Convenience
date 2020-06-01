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

        public WorkFlowInstanceStateEnum WorkFlowInstanceState { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatedUserId { get; set; }

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
        NoCommitted = 1,
        CirCulation = 2,
        ReturnBack = 3,
        End = 4
    }
}
