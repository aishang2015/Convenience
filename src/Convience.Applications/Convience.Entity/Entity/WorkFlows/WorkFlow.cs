using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.Entity.Entity.WorkFlows
{
    public class WorkFlow
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Describe { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatedUser { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string UpdatedUser { get; set; }

        #region

        public List<WorkFlowLink> WorkFlowRoutes { get; set; }

        public List<WorkFlowNode> WorkFlowNodes { get; set; }

        public List<WorkFlowFormControl> WorkFlowFormControls { get; set; }

        #endregion
    }
}
