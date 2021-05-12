import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { WorkFlowGroup } from '../model/workflowGroup';
import { NzTreeNodeOptions, NzTreeNode, NzFormatEmitEvent } from 'ng-zorro-antd/tree/ng-zorro-antd-tree';
import { WorkflowGroupService } from 'src/app/business/workflow/workflowgroup.service';

@Component({
  selector: 'app-workflow-group-tree',
  templateUrl: './workflow-group-tree.component.html',
  styleUrls: ['./workflow-group-tree.component.less']
})
export class WorkflowGroupTreeComponent implements OnInit {

  @Output()
  selectedNodeChanged = new EventEmitter<WorkFlowGroup[]>();

  @Output()
  loadData = new EventEmitter<NzTreeNodeOptions[]>();

  @Output()
  nodeChecked = new EventEmitter<string>();

  nodes: NzTreeNodeOptions[] = [];

  data: WorkFlowGroup[] = [];

  selectedNode: NzTreeNode;

  constructor(private _workflowGroupService: WorkflowGroupService) { }

  ngOnInit(): void {
    this.initNodes();
  }

  initNodes() {
    let nodes: NzTreeNodeOptions[] = [{ title: '工作流类别', key: null, icon: 'global', expanded: true, children: [] }];
    this._workflowGroupService.getAll().subscribe((result: any) => {
      this.data = result;
      this.makeNodes(null, nodes[0], this.data);
      this.nodes = nodes;
      this.loadData.emit(this.nodes);

      let selectedData = this.data.filter(department => department.upId == this.selectedNode?.key);
      this.selectedNodeChanged.emit(selectedData);
    });
  }

  makeNodes(upId, node, departments: WorkFlowGroup[]) {
    var ms = departments.filter(department => department.upId == upId);
    ms.forEach(department => {
      let data = { title: department.name, key: department.id, icon: 'appstore', children: [] };
      this.makeNodes(department.id, data, departments);
      node.children.push(data);
    });
  }

  treeClick(event: NzFormatEmitEvent) {
    this.selectedNode = event.keys.length > 0 ? event.node : null;
    let selectedData = this.data.filter(department => department.upId == this.selectedNode?.key);
    this.selectedNodeChanged.emit(selectedData);
    this.nodeChecked.emit(this.selectedNode?.key?.toString());
  }

}
