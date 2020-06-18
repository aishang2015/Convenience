using Convience.Entity.Entity.WorkFlows;
using System.Collections.Generic;

namespace Convience.Model.Models.WorkFlowManage
{
    public class WorkFlowLinkViewModel
    {
        public int Id { get; set; }

        public int WorkFlowId { get; set; }

        public string SourceId { get; set; }

        public string TargetId { get; set; }

        public string Connection { get; set; }
    }

    public class WorkFlowLinkResult : WorkFlowLinkViewModel { }

    public class WorkFlowNodeViewModel
    {
        public int Id { get; set; }

        public int WorkFlowId { get; set; }

        public string Name { get; set; }

        public NodeTypeEnum NodeType { get; set; }

        public string DomId { get; set; }

        public int Top { get; set; }

        public int Left { get; set; }

        public HandleModeEnum HandleMode { get; set; }

        public string Handlers { get; set; }

        public string Position { get; set; }

        public string Department { get; set; }
    }

    public class WorkFlowNodeResult : WorkFlowNodeViewModel { }

    public class WorkFlowConditionViewModel
    {
        public int? Id { get; set; }

        public int WorkFlowId { get; set; }

        public string SourceId { get; set; }

        public string TargetId { get; set; }

        public int FormControlId { get; set; }

        public CompareModeEnum CompareMode { get; set; }

        public string CompareValue { get; set; }
    }

    public class WorkFlowConditionResult : WorkFlowConditionViewModel { }

    public class WorkFlowFlowViewModel
    {
        public int WorkFlowId { get; set; }

        public IEnumerable<WorkFlowLinkViewModel> WorkFlowLinkViewModels { get; set; }

        public IEnumerable<WorkFlowNodeViewModel> WorkFlowNodeViewModels { get; set; }

        public IEnumerable<WorkFlowConditionViewModel> WorkFlowConditionViewModels { get; set; }
    }

    public class WorkFlowFlowResult
    {
        public IEnumerable<WorkFlowLinkResult> WorkFlowLinkResults { get; set; }

        public IEnumerable<WorkFlowNodeResult> WorkFlowNodeResults { get; set; }

        public IEnumerable<WorkFlowConditionResult> WorkFlowConditionResults { get; set; }
    }
}
