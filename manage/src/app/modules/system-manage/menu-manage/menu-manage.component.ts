import { Component, OnInit } from '@angular/core';
import { NzTreeNodeOptions } from 'ng-zorro-antd';

@Component({
  selector: 'app-menu-manage',
  templateUrl: './menu-manage.component.html',
  styleUrls: ['./menu-manage.component.scss']
})
export class MenuManageComponent implements OnInit {

  nodes: NzTreeNodeOptions[] = [];

  constructor() { }

  ngOnInit(): void {
    this.nodes.push(
      { title: '权限管理系统', key: '0', icon: 'global' }
    );
  }

  treeClick() {
  }

  initNodes(){
    let result:any;
    result.forEach(menu => {
      if(!menu.upid){
        this.nodes.push(menu);
      }
    });
  }

  makeNodes(menu){
    // 查找子节点
    // 循环每个子节点
    // 递归 menu.children.push(makeNodes(menu))
    // 
   
  }

}
