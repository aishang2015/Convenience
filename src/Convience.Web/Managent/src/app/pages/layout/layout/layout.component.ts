import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { NzModalService, NzModalRef, NzMessageService } from 'ng-zorro-antd';
import { AccountService } from 'src/app/business/account.service';
import { StorageService } from 'src/app/services/storage.service';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

  menuTree = [
    {
      canOperate: 'dashaboard', routerLink: '/dashboard', iconType: 'dot-chart', firstBreadcrumb: '仪表盘', lastBreadcrumb: '', name: '仪表盘',
      children: []
    },
    {
      canOperate: 'systemmanage', routerLink: '', iconType: 'setting', firstBreadcrumb: '', lastBreadcrumb: '', name: '系统管理',
      children: [
        { canOperate: 'userManage', routerLink: '/system/user', iconType: 'user', firstBreadcrumb: '系统管理', lastBreadcrumb: '用户管理', name: '用户管理', },
        { canOperate: 'roleManage', routerLink: '/system/role', iconType: 'idcard', firstBreadcrumb: '系统管理', lastBreadcrumb: '角色管理', name: '角色管理', },
        { canOperate: 'menuManage', routerLink: '/system/menu', iconType: 'menu', firstBreadcrumb: '系统管理', lastBreadcrumb: '菜单管理', name: '菜单管理', },
      ]
    },
    {
      canOperate: 'groupmanage', routerLink: '', iconType: 'team', firstBreadcrumb: '', lastBreadcrumb: '', name: '组织管理',
      children: [
        { canOperate: 'employeeManage', routerLink: '/group/employee', iconType: 'reconciliation', firstBreadcrumb: '组织管理', lastBreadcrumb: '员工管理', name: '员工管理', },
        { canOperate: 'positionManage', routerLink: '/group/position', iconType: 'credit-card', firstBreadcrumb: '组织管理', lastBreadcrumb: '职位管理', name: '职位管理', },
        { canOperate: 'departmentManage', routerLink: '/group/department', iconType: 'apartment', firstBreadcrumb: '组织管理', lastBreadcrumb: '部门管理', name: '部门管理', },
      ]
    },
    {
      canOperate: 'workflow', routerLink: '', iconType: 'fork', firstBreadcrumb: '', lastBreadcrumb: '', name: '工作流',
      children: [
        { canOperate: 'myWorkflow', routerLink: '/workflow/myFlow', iconType: 'credit-card', firstBreadcrumb: '组织管理', lastBreadcrumb: '我创建的', name: '我创建的', },
        { canOperate: 'handledWorkflow', routerLink: '/workflow/handledFlow', iconType: 'highlight', firstBreadcrumb: '组织管理', lastBreadcrumb: '我处理的', name: '我处理的', },
        { canOperate: 'workflowManage', routerLink: '/workflow/workflowManage', iconType: 'reconciliation', firstBreadcrumb: '组织管理', lastBreadcrumb: '工作流管理', name: '工作流管理', },
      ]
    },
    {
      canOperate: 'contentmanage', routerLink: '', iconType: 'book', firstBreadcrumb: '', lastBreadcrumb: '', name: '内容管理',
      children: [
        { canOperate: 'articleManage', routerLink: '/content/article', iconType: 'align-left', firstBreadcrumb: '内容管理', lastBreadcrumb: '文章管理', name: '文章管理', },
        { canOperate: 'columnManage', routerLink: '/content/column', iconType: 'database', firstBreadcrumb: '内容管理', lastBreadcrumb: '栏目管理', name: '栏目管理', },
        { canOperate: 'fileManage', routerLink: '/content/file', iconType: 'file', firstBreadcrumb: '内容管理', lastBreadcrumb: '文件管理', name: '文件管理', },
        { canOperate: 'dicManage', routerLink: '/content/dic', iconType: 'book', firstBreadcrumb: '内容管理', lastBreadcrumb: '字典管理', name: '字典管理', },
      ]
    },
    {
      canOperate: 'saasmanage', routerLink: '', iconType: 'deployment-unit', firstBreadcrumb: '', lastBreadcrumb: '', name: 'SAAS管理',
      children: [
        { canOperate: 'tenantManage', routerLink: '/saas/tenant', iconType: 'user', firstBreadcrumb: 'SAAS管理', lastBreadcrumb: '租户管理', name: '租户管理', },
      ]
    },
    {
      canOperate: 'systemtool', routerLink: '', iconType: 'tool', firstBreadcrumb: '', lastBreadcrumb: '', name: '系统工具',
      children: [
        { canOperate: 'code', routerLink: '/tool/code', iconType: 'fund-view', firstBreadcrumb: '内容管理', lastBreadcrumb: '代码生成', name: '代码生成', },
        { canOperate: 'swagger', routerLink: '/tool/swagger', iconType: 'api', firstBreadcrumb: '内容管理', lastBreadcrumb: 'Swagger', name: 'Swagger', },
        { canOperate: 'hangfire', routerLink: '/tool/hangfire', iconType: 'fund-view', firstBreadcrumb: '内容管理', lastBreadcrumb: 'Hangfire', name: 'Hangfire', },
        { canOperate: 'cap', routerLink: '/tool/cap', iconType: 'forward', firstBreadcrumb: '内容管理', lastBreadcrumb: 'CAP', name: 'CAP', },
      ]
    },

  ];

  breadcrumbInfo: string[] = ['仪表盘'];
  isCollapsed;
  name;
  avatar;

  @ViewChild('editPwdTitleTpl', { static: true })
  editPwdTitleTpl;

  @ViewChild('editPwdContentTpl', { static: true })
  editPwdContentTpl;

  modifyForm: FormGroup;

  isLoading: boolean;

  modalRef: NzModalRef;

  equalValidator = (control: FormControl): { [key: string]: any } | null => {
    const newPassword = this.modifyForm?.get('newPassword').value;
    const confirmPassword = control.value;
    return newPassword === confirmPassword ? null : { 'notEqual': true };
  };

  constructor(
    private _storageService: StorageService,
    private _router: Router,
    private _formBuilder: FormBuilder,
    private _modalService: NzModalService,
    private _messageService: NzMessageService,
    private _accountService: AccountService,
    private _signalRService: SignalrService) { }

  ngOnInit() {
    this.name = this._storageService.Name;
    this.avatar = this._storageService.Avatar;

    // this._signalRService.addReceiveMessageHandler("newMsg", (value) => {
    //   console.log(value);
    // });
    // this._signalRService.start();
  }

  logout() {
    this._storageService.removeUserToken();
    this._router.navigate(['/account/login']);
  }

  setBreadcrumb(first: string, ...rest: string[]) {
    this.breadcrumbInfo = [];
    this.breadcrumbInfo.push(first);
    rest.forEach(element => {
      this.breadcrumbInfo.push(element);
    });
  }

  getImgUrl(name) {
    return `/assets/avatars/${name}.png`;
  }

  modifyPwd() {
    this.modifyForm = this._formBuilder.group({
      // key: value,validators,asyncvalidators,updateOn
      userName: [{ value: this._storageService.userName, disabled: true }],
      oldPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required, this.equalValidator]]
    });
    this.modalRef = this._modalService.create({
      nzTitle: this.editPwdTitleTpl,
      nzContent: this.editPwdContentTpl,
      nzFooter: null,
      nzMaskClosable: false
    });
  }

  submitForm() {
    for (const i in this.modifyForm.controls) {
      this.modifyForm.controls[i].markAsDirty();
      this.modifyForm.controls[i].updateValueAndValidity();
    }
    if (this.modifyForm.valid) {
      this.isLoading = true;
      this._accountService.modifyPassword(this.modifyForm.controls['oldPassword'].value, this.modifyForm.controls['newPassword'].value)
        .subscribe(
          result => {
            this.modifyForm.reset();
            this._messageService.success("密码修改成功！");
            this.modalRef.close();
          },
          error => {
            this.isLoading = false;
          },
          () => {
            this.isLoading = false;
          }
        )

    }
  }
}
