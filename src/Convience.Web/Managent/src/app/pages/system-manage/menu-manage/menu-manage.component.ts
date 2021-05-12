import { Component, OnInit, TemplateRef } from '@angular/core';
import { Menu } from '../model/menu';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MenuService } from 'src/app/business/system-manage/menu.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { NzTreeNodeOptions, NzTreeNode, NzFormatEmitEvent } from 'ng-zorro-antd/tree';

@Component({
  selector: 'app-menu-manage',
  templateUrl: './menu-manage.component.html',
  styleUrls: ['./menu-manage.component.less']
})
export class MenuManageComponent implements OnInit {

  nodes: NzTreeNodeOptions[] = [];

  selectedNode: NzTreeNode;

  editForm: FormGroup = new FormGroup({});

  data: Menu[] = [];

  displayData: Menu[] = [];

  editedMenu: Menu = {};

  modalRef: NzModalRef;

  constructor(
    private _modalService: NzModalService,
    private _formBuilder: FormBuilder,
    private _menuService: MenuService,
    private _messageService: NzMessageService) { }

  ngOnInit(): void {
    this.initNodes();
  }

  initNodes() {
    let nodes: NzTreeNodeOptions[] = [{ title: '菜单管理', key: null, icon: 'global', expanded: true, children: [] }];
    this._menuService.get().subscribe((result: any) => {
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
        this._messageService.warning("按钮和链接类型节点无法添加子元素！");
        return;
      }
    }
    this.editedMenu = new Menu();
    this.editForm = this._formBuilder.group({
      upMenu: [this.selectedNode?.key],
      name: [null, [Validators.required, Validators.maxLength(10)]],
      identification: [null],
      permission: [null],
      type: [0],
      route: [null],
      sort: [null, [Validators.required]]
    });
    this.modalRef = this._modalService.create({
      nzTitle: title,
      nzContent: content,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  edit(title: TemplateRef<{}>, content: TemplateRef<{}>, menu: Menu) {
    this.editedMenu = menu;
    console.log(menu);
    this.editForm = this._formBuilder.group({
      upMenu: [{ value: Number(menu.upId), disabled: true }],
      //upMenu: [Number(menu.upId)],
      name: [menu.name, [Validators.required, Validators.maxLength(10)]],
      identification: [menu.identification],
      permission: [menu.permission],
      type: [menu.type],
      route: [menu.route],
      sort: [menu.sort, [Validators.required]]
    });
    this.modalRef = this._modalService.create({
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
      menu.upId = this.editForm.value["upMenu"];
      if (this.editedMenu.id) {
        menu.id = this.editedMenu.id;
        this._menuService.update(menu).subscribe(result => {
          this._messageService.success("修改成功！");
          this.initNodes();
          this.modalRef.close();
        });
      } else {
        this._menuService.add(menu).subscribe(result => {
          this._messageService.success("添加成功！");
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
    this._modalService.confirm({
      nzTitle: '是否删除该菜单?',
      nzContent: '删除菜单会导致相关用户的权限无法使用，请谨慎操作！',
      nzOnOk: () =>
        this._menuService.delete(id).subscribe(result => {
          this.initNodes();
          this._messageService.success("删除成功！");
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
