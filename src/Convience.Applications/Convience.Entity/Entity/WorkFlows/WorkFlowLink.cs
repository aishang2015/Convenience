using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.Entity.Entity.WorkFlows
{
    public class WorkFlowLink
    {
        public int Id { get; set; }

        public int SourceId { get; set; }

        public int TargetId { get; set; }

        public string Connection { get; set; }

        #region

        public int WorkFlowId { get; set; }

        public WorkFlow WorkFlow { get; set; }

        #endregion
    }
}
