import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';

import { DepartmentService } from 'src/app/business/group-manage/department.service';
import { NzTreeNodeOptions, NzTreeNode, NzFormatEmitEvent } from 'ng-zorro-antd/tree';
import { Department } from 'src/app/pages/group-manage/model/department';

@Component({
  selector: 'app-department-tree',
  templateUrl: './department-tree.component.html',
  styleUrls: ['./department-tree.component.less']
})
export class DepartmentTreeComponent implements OnInit {

  @Output()
  selectedNodeChanged = new EventEmitter<Department[]>();

  @Output()
  loadData = new EventEmitter<NzTreeNodeOptions[]>();

  @Output()
  nodeChecked = new EventEmitter<string>();

  nodes: NzTreeNodeOptions[] = [];

  data: Department[] = [];

  selectedNode: NzTreeNode;

  constructor(private _departmentService: DepartmentService) { }

  ngOnInit(): void {
    this.initNodes();
  }

  initNodes() {
    let nodes: NzTreeNodeOptions[] = [{ title: '组织结构', key: null, icon: 'global', expanded: true, children: [] }];
    this._departmentService.getAll().subscribe((result: any) => {
      this.data = result;
      this.makeNodes(null, nodes[0], this.data);
      this.nodes = nodes;
      this.loadData.emit(this.nodes);

      let selectedData = this.data.filter(department => department.upId == this.selectedNode?.key);
      this.selectedNodeChanged.emit(selectedData);
    });
  }

  makeNodes(upId, node, departments: Department[]) {
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
