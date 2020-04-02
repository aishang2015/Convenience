import { Component, OnInit, TemplateRef } from '@angular/core';
import { NzTreeNodeOptions, NzFormatEmitEvent, NzModalRef, NzModalService, NzTreeNode, NzMessageService } from 'ng-zorro-antd';
import { Menu } from '../model/menu';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MenuService } from 'src/app/common/services/menu.service';

@Component({
  selector: 'app-menu-manage',
  templateUrl: './menu-manage.component.html',
  styleUrls: ['./menu-manage.component.scss']
})
export class MenuManageComponent implements OnInit {

  nodes: NzTreeNodeOptions[] = [];

  selectedNode: NzTreeNode;

  editForm: FormGroup = new FormGroup({});

  data: Menu[] = [];

  displayData: Menu[] = [];

  editedMenu: Menu = {};

  modalRef: NzModalRef;

  constructor(private modalService: NzModalService,
    private fb: FormBuilder,
    private menuService: MenuService,
    private messageService: NzMessageService) { }

  ngOnInit(): void {
    this.initNodes();
  }

  initNodes() {
    let nodes: NzTreeNodeOptions[] = [{ title: '菜单管理', key: null, icon: 'global', expanded: true, children: [] }];
    this.menuService.get().subscribe((result: any) => {
      this.data = result;
      this.makeNodes(null, nodes[0], this.data);
      this.nodes = nodes;
      this.displayData = this.data.filter(menu => menu.upId == this.selectedNode?.key);
    });
  }

  makeNodes(upId, node, menus: Menu[]) {
    var ms = menus.filter(menu => menu.upId == upId);
    ms.forEach(menu => {
      let data = { title: menu.name, key: menu.id, icon: this.menuIcon(menu.type), children: [], isLeaf: menu.type == 2 || menu.type == 3 };
      this.makeNodes(menu.id, data, menus);
      node.children.push(data);
    });
  }

  add(title: TemplateRef<{}>, content: TemplateRef<{}>) {
    if (this.selectedNode) {
      let selectMenu: Menu = this.data.find(menu => menu.id.toString() == this.selectedNode?.key);
      if (selectMenu && (selectMenu.type == 2 || selectMenu.type == 3)) {
        this.messageService.warning("按钮和链接类型节点无法添加子元素！");
        return;
      }
    }
    this.editedMenu = new Menu();
    this.editForm = this.fb.group({
      upMenu: [{ value: this.getUpperMenuBySelect(), disabled: true }],
      name: [null, [Validators.required, Validators.maxLength(10)]],
      identification: [null],
      permission: [null],
      type: [0],
      route: [null],
      sort: [null, [Validators.required]]
    });
    this.modalRef = this.modalService.create({
      nzTitle: title,
      nzContent: content,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  edit(title: TemplateRef<{}>, content: TemplateRef<{}>, menu: Menu) {
    this.editedMenu = menu;
    this.editForm = this.fb.group({
      upMenu: [{ value: this.getUpperMenuById(menu.upId), disabled: true }],
      name: [menu.name, [Validators.required, Validators.maxLength(10)]],
      identification: [menu.identification],
      permission: [menu.permission],
      type: [menu.type],
      route: [menu.route],
      sort: [menu.sort, [Validators.required]]
    });
    this.modalRef = this.modalService.create({
      nzTitle: title,
      nzContent: content,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      let menu = new Menu();
      menu.identification = this.editForm.value["identification"];
      menu.name = this.editForm.value["name"];
      menu.permission = this.editForm.value["permission"];
      menu.route = this.editForm.value["route"];
      menu.sort = this.editForm.value["sort"];
      menu.type = this.editForm.value["type"];
      menu.upId = this.selectedNode?.key?.toString();
      if (this.editedMenu.id) {
        menu.id = this.editedMenu.id;
        this.menuService.update(menu).subscribe(result => {
          this.messageService.success("修改成功！");
          this.initNodes();
          this.modalRef.close();
        });
      } else {
        this.menuService.add(menu).subscribe(result => {
          this.messageService.success("添加成功！");
          this.initNodes();
          this.modalRef.close();
        });
      }
    }
  }

  cancelEdit() {
    this.modalRef.close();
  }

  remove(id: string) {
    this.modalService.confirm({
      nzTitle: '是否删除该菜单?',
      nzContent: null,
      nzOnOk: () =>
        this.menuService.delete(id).subscribe(result => {
          this.initNodes();
          this.messageService.success("删除成功！");
        })
    });
  }

  getUpperMenuBySelect() {
    var key = this.selectedNode?.key;
    var selectedMeun = this.data.find(menu => menu.id.toString() == key);
    return selectedMeun?.name;
  }

  getUpperMenuById(key) {
    var selectedMeun = this.data.find(menu => menu.id.toString() == key);
    return selectedMeun?.name;
  }

  treeClick(event: NzFormatEmitEvent) {
    this.selectedNode = event.keys.length > 0 ? event.node : null;
    this.displayData = this.data.filter(menu => menu.upId == this.selectedNode?.key);
  }

  menuIcon(type) {
    var result = '';
    switch (type) {
      case 1:
        result = 'menu';
        break;
      case 2:
        result = 'plus-square';
        break;
      case 3:
        result = 'link';
        break;
      default:
        result = 'appstore';
        break;
    }
    return result;
  }

}
