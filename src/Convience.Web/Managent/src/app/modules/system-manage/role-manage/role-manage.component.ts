import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Role } from '../model/role';
import { NzModalService, NzModalRef, NzMessageService, NzTreeNodeOptions, NzTreeSelectComponent } from 'ng-zorro-antd';
import { RoleService } from 'src/app/services/role.service';
import { Menu } from '../model/menu';
import { MenuService } from 'src/app/services/menu.service';

@Component({
  selector: 'app-role-manage',
  templateUrl: './role-manage.component.html',
  styleUrls: ['./role-manage.component.scss']
})
export class RoleManageComponent implements OnInit {

  @ViewChild('menuTree', { static: false }) menuTree: NzTreeSelectComponent;
  
  searchForm: FormGroup = new FormGroup({});
  editForm: FormGroup = new FormGroup({});

  nodes: NzTreeNodeOptions[] = [];

  tplModal: NzModalRef;

  data: Role[] = [];

  editedRole: Role = new Role();

  page: number = 1;
  size: number = 10;
  total: number = 0;

  constructor(private fb: FormBuilder,
    private modalService: NzModalService,
    private roleService: RoleService,
    private menuService: MenuService,
    private messageService: NzMessageService) { }

  ngOnInit(): void {
    this.searchForm = this.fb.group({
      roleName: [""]
    });
    this.refresh();
    this.initNodes();
  }

  refresh() {
    this.roleService.getRoles(null, this.page, this.size)
      .subscribe((result: any) => { this.data = result['data']; this.total = result['count']; });
  }

  initNodes() {
    let nodes: NzTreeNodeOptions[] = [];
    this.menuService.get().subscribe((result: any) => {
      this.makeNodes(null, nodes, result);
      this.nodes = nodes;
    });
  }

  makeNodes(upId, node, menus: Menu[]) {
    var ms = menus.filter(menu => menu.upId == upId);
    ms.forEach(menu => {
      let data = { title: menu.name, key: menu.id.toString(), children: [], isLeaf: menu.type == 2 || menu.type == 3 };
      this.makeNodes(menu.id, data.children, menus);
      node.push(data);
    });
  }

  submitSearch() {
    let key = this.searchForm.value["roleName"];
    this.roleService.getRoles(key, this.page, this.size)
      .subscribe((result: any) => { this.data = result['data']; this.total = result['count']; });
  }

  addRole(title: TemplateRef<{}>, content: TemplateRef<{}>) {
    this.editedRole = new Role();
    this.editForm = this.fb.group({
      roleName: [this.editedRole.name, [Validators.required, Validators.maxLength(15)]],
      remark: [this.editedRole.remark, [Validators.maxLength(30)]],
      menus: [[]]
    });
    this.tplModal = this.modalService.create({
      nzTitle: title,
      nzContent: content,
      nzFooter: null,
    });
  }

  editRole(title: TemplateRef<{}>, content: TemplateRef<{}>, role: Role) {
    this.roleService.getRole(role.id).subscribe(reuslt => {
      this.editedRole = reuslt;
      this.editForm = this.fb.group({
        roleName: [this.editedRole.name, [Validators.required, Validators.maxLength(15)]],
        remark: [this.editedRole.remark, [Validators.maxLength(30)]],
        menus: [this.editedRole.menus?.split(',')]
      });
      this.tplModal = this.modalService.create({
        nzTitle: title,
        nzContent: content,
        nzFooter: null,
      });

    });
  }

  removeRole(roleName: string) {
    this.modalService.confirm({
      nzTitle: '是否删除该角色?',
      nzContent: null,
      nzOnOk: () =>
        this.roleService.deleteRole(roleName).subscribe(result => {
          this.refresh();
          this.messageService.success("删除成功！");
        })
    });
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      this.editedRole.name = this.editForm.value['roleName'];
      this.editedRole.remark = this.editForm.value['remark'];
      this.editedRole.menus = this.editForm.value['menus'].join(',');
      if (this.editedRole.id) {
        this.roleService.updateRole(this.editedRole)
          .subscribe(result => {
            this.refresh(); this.tplModal.close();
            this.messageService.success("更新成功！");
          });
      } else {
        this.roleService.addRole(this.editedRole)
          .subscribe(result => {
            this.refresh(); this.tplModal.close();
            this.messageService.success("添加成功！");
          });
      }
    }
  }
  cancelEdit() {
    this.tplModal.close();
  }

  pageChange() {
    this.refresh();
  }

  sizeChange() {
    this.page = 1;
    this.refresh();
  }


}
