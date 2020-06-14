export class WorkflowNode {

    // StartNode = 0,
    // WorkNode = 1,
    // EndNode = 99,
    nodeType: number;

    domId: string;
    name: string;
    top: number;
    left: number;

    // Personnel = 1, // 指定人员模式
    // Position = 2, // 指定职位模式
    // Leader = 3, // 指定部门负责人模式
    // UserLeader = 4, // 指定发起人部门负责人模式
    // UpLeader = 5, // 指定发起人上级部门负责人模式
    handleMode: number;
    handlers: string; // 1
    position: string; // 2
    department: string; // 3
}