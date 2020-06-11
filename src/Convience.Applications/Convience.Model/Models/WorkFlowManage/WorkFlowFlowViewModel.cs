using Convience.Entity.Entity.WorkFlows;
using System.Collections.Generic;

namespace Convience.Model.Models.WorkFlowManage
{
    public class WorkFlowLinkViewModel
    {
        public int Id { get; set; }

        public int WorkFlowId { get; set; }

        public int SourceId { get; set; }

        public int TargetId { get; set; }

        public string Connection { get; set; }
    }

    public class WorkFlowLinkResult : WorkFlowLinkViewModel { }

    public class WorkFlowNodeViewModel
    {
        public int Id { get; set; }

        public int WorkFlowId { get; set; }

        public string Name { get; set; }

        public NodeTypeEnum NodeType { get; set; }

        public string ElementId { get; set; }

        public int Top { get; set; }

        public int Left { get; set; }

        public HandleModeEnum HandleMode { get; set; }

        public string Handlers { get; set; }

        public string Position { get; set; }

        public string Department { get; set; }
    }

    public class WorkFlowNodeResult : WorkFlowNodeViewModel { }

    public class WorkFlowFlowViewModel
    {
        public int WorkFlowId { get; set; }

        public IEnumerable<WorkFlowLinkViewModel> WorkFlowLinkViewModels { get; set; }

        public IEnumerable<WorkFlowNodeViewModel> WorkFlowNodeViewModels { get; set; }
    }

    public class WorkFlowFlowResult
    {
        public IEnumerable<WorkFlowLinkResult> WorkFlowLinkResults { get; set; }

        public IEnumerable<WorkFlowNodeResult> WorkFlowNodeResults { get; set; }
    }
}
