using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

using System;
using System.Collections.Generic;

namespace Convience.Entity.Entity.WorkFlows
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class WorkFlow
    {
        public int Id { get; set; }

        public int WorkFlowGroupId { get; set; }

        public string Name { get; set; }

        public string Describe { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatedUser { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string UpdatedUser { get; set; }

        // 是否发布
        public bool IsPublish { get; set; }

        #region

        public List<WorkFlowLink> WorkFlowLinks { get; set; }

        public List<WorkFlowNode> WorkFlowNodes { get; set; }

        public List<WorkFlowCondition> WorkFlowConditions { get; set; }

        public WorkFlowForm WorkFlowForm { get; set; }

        public List<WorkFlowFormControl> WorkFlowFormControls { get; set; }

        #endregion
    }
}
