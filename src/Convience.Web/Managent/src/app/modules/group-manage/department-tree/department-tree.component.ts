import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { NzTreeNodeOptions, NzFormatEmitEvent, NzTreeNode } from 'ng-zorro-antd';
import { Department } from '../model/department';
import { DepartmentService } from 'src/app/services/department.service';

@Component({
  selector: 'app-department-tree',
  templateUrl: './department-tree.component.html',
  styleUrls: ['./department-tree.component.scss']
})
export class DepartmentTreeComponent implements OnInit {

  @Output()
  nodeClick = new EventEmitter<Department[]>();

  nodes: NzTreeNodeOptions[] = [];

  data: Department[] = [];

  selectedNode: NzTreeNode;

  constructor(private departmentService: DepartmentService) { }

  ngOnInit(): void {
    this.initNodes();
  }

  initNodes() {
    let nodes: NzTreeNodeOptions[] = [{ title: '组织结构', key: null, icon: 'global', expanded: true, children: [] }];
    this.departmentService.get().subscribe((result: any) => {
      this.data = result;
      this.makeNodes(null, nodes[0], this.data);
      this.nodes = nodes;
    });
  }

  makeNodes(upId, node, menus: Department[]) {
    var ms = menus.filter(menu => menu.upId == upId);
    ms.forEach(menu => {
      let data = { title: menu.name, key: menu.id, icon: 'appstore', children: [] };
      this.makeNodes(menu.id, data, menus);
      node.children.push(data);
    });
  }

  treeClick(event: NzFormatEmitEvent) {
    this.selectedNode = event.keys.length > 0 ? event.node : null;
    let selectedData = this.data.filter(menu => menu.upId == this.selectedNode?.key);
    this.nodeClick.emit(selectedData);
  }

}
