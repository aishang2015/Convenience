using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Convience.Entity.Entity.WorkFlows
{
    public class WorkFlowNode
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public NodeType NodeType { get; set; }

        // DomId
        public string ElementId { get; set; }

        public int Top { get; set; }

        public int Left { get; set; }

        public HandleMode HandleMode { get; set; }

        // 处理人ID
        public string Handlers { get; set; }

        // 指定的处理岗位ID
        public string Position { get; set; }

        // 指定的负责人的部门ID
        public string Department { get; set; }

        #region

        public int WorkFlowId { get; set; }

        public WorkFlow WorkFlow { get; set; }

        #endregion
    }

    public enum NodeType
    {
        StartNode = 0,
        WorkNode = 1,
        EndNode = 99,
    }

    /// <summary>
    /// 办理模式
    /// </summary>
    public enum HandleMode
    {
        Personnel = 1, // 指定人员模式
        Position = 2, // 指定职位模式
        Leader = 3, // 指定部门负责人模式
        UserLeader = 4, // 指定发起人部门负责人模式
        UpLeader = 5, // 指定发起人上级部门负责人模式
    }
}
