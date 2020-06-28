using System;

namespace Convience.Model.Models.WorkFlowManage
{
    public class WorkFlowViewModel
    {
        public int Id { get; set; }

        public int WorkFlowGroupId { get; set; }

        public string Name { get; set; }

        public string Describe { get; set; }

        public bool IsPublish { get; set; }
    }

    public class WorkFlowResultModel : WorkFlowViewModel
    {
        public DateTime CreatedTime { get; set; }

        public string CreatedUser { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string UpdatedUser { get; set; }
    }

    public class WorkFlowQueryModel
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public int WorkFlowGroupId { get; set; }

        public bool? IsPublish { get; set; }
    }
}
