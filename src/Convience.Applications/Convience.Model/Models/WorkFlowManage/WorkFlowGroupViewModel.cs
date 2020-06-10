using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.Model.Models.WorkFlowManage
{
    public class WorkFlowGroupViewModel
    {
        public int Id { get; set; }

        public string UpId { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }
    }

    public class WorkFlowGroupResult : WorkFlowGroupViewModel
    {

        public DateTime CreatedTime { get; set; }

        public string CreatedUser { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string UpdatedUser { get; set; }
    }

}
