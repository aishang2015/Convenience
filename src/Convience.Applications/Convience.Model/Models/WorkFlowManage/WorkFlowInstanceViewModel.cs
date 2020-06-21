using Convience.Entity.Entity.WorkFlows;

using System;
using System.Collections.Generic;

namespace Convience.Model.Models.WorkFlowManage
{
    public class WorkFlowInstanceViewModel
    {
        public int WorkFlowId { get; set; }
    }

    public class WorkFlowInstanceResult : WorkFlowInstanceViewModel
    {
        public int Id { get; set; }

        public string WorkFlowName { get; set; }

        public WorkFlowInstanceStateEnum WorkFlowInstanceState { get; set; }

        public int CurrentNodeId { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatedUserAccount { get; set; }

        public string CreatedUserName { get; set; }
    }

    public class WorkFlowInstanceValueViewModel
    {
        public int WorkFlowInstanceId { get; set; }

        public string FormControlId { get; set; }

        public string Value { get; set; }
    }

    public class InstanceValuesViewModel
    {
        public int WorkFlowInstanceId { get; set; }

        public IEnumerable<WorkFlowInstanceValueViewModel> Values { get; set; }
    }

    public class WorkFlowInstanceValueResult : WorkFlowInstanceValueViewModel { }

    public class WorkFlowInstanceHandleViewModel
    {
        public int WorkFlowInstanceId { get; set; }

        public bool IsPass { get; set; }

        public string HandleComment { get; set; }
    }

    public class WorkFlowInstanceHandleResult
    {
        public int NodeId { get; set; }

        public string HandlePeople { get; set; }

        public string HandleComment { get; set; }

        public HandleStateEnum HandleState { get; set; }

        public DateTime HandleTime { get; set; }

        public int WorkFlowInstanceId { get; set; }
    }

}
