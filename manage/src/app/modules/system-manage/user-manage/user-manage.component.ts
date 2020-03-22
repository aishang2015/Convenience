import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { User } from '../model/user';
import { UserService } from 'src/app/common/services/user.service';
import { NzModalService, NzModalRef, NzMessageService } from 'ng-zorro-antd';
import { RoleService } from 'src/app/common/services/role.service';

@Component({
  selector: 'app-user-manage',
  templateUrl: './user-manage.component.html',
  styleUrls: ['./user-manage.component.scss']
})
export class UserManageComponent implements OnInit {

  searchForm: FormGroup = new FormGroup({});
  editForm: FormGroup = new FormGroup({});;
  isNewUser: boolean;

  editedUser: User;

  tplModal: NzModalRef;

  page: number = 1;
  size: number = 10;
  total: number = 0;

  data: User[] = [];
  roleNames: string[] = [];

  constructor(private formBuilder: FormBuilder,
    private userService: UserService,
    private roleService: RoleService,
    private messageService: NzMessageService,
    private modalService: NzModalService) { }

  ngOnInit(): void {
    this.initRoleList();
    this.refresh();
    this.searchForm = this.formBuilder.group({
      userName: [null],
      phoneNumber: [null],
      name: [null],
      roleName: [null],
    });
  }

  initRoleList() {
    this.roleService.getRoleList().subscribe((result: any) => this.roleNames = result);
  }

  refresh() {
    this.userService.getUsers(this.page, this.size, this.searchForm.value['userName'],
      this.searchForm.value['phoneNumber'], this.searchForm.value['name'], this.searchForm.value['roleName'])
      .subscribe(result => {
        this.data = result['data'];
        this.total = result['count'];
      });
  }

  add(title: TemplateRef<{}>, content: TemplateRef<{}>) {
    this.isNewUser = true;
    this.editedUser = new User();
    this.editForm = this.formBuilder.group({
      userName: ['', [Validators.required, Validators.maxLength(15)]],
      password: ['', [Validators.required, Validators.maxLength(30)]],
      name: ['', [Validators.required, Validators.maxLength(10)]],
      phoneNumber: ['', [Validators.pattern('^1(3|4|5|7|8)[0-9]{9}$')]],
      roleNames: [[]],
      sex: [0],
      isActive: [false],
    });
    this.tplModal = this.modalService.create({
      nzTitle: title,
      nzContent: content,
      nzFooter: null,
      nzClosable: true,
      nzMaskClosable: false
    });
  }


  edit(title: TemplateRef<{}>, content: TemplateRef<{}>, user: User) {
    this.userService.getUser(user.id).subscribe(user => {
      this.isNewUser = false;
      this.editedUser = user;
      this.editForm = this.formBuilder.group({
        userName: [user['userName'], [Validators.required, Validators.maxLength(15)]],
        password: [null, [Validators.maxLength(30)]],
        name: [user['name'], [Validators.required, Validators.maxLength(10)]],
        phoneNumber: [user['phoneNumber'], [Validators.pattern('^1(3|4|5|7|8)[0-9]{9}$')]],
        roleNames: [user['roleNames'].split(',')],
        sex: [user['sex']],
        isActive: [user['isActive']],
      });
      this.tplModal = this.modalService.create({
        nzTitle: title,
        nzContent: content,
        nzFooter: null,
        nzClosable: true,
        nzMaskClosable: false
      });
    });
  }


  remove(id: string) {
    this.modalService.confirm({
      nzTitle: '是否删除该用户?',
      nzContent: null,
      nzOnOk: () =>
        this.userService.delete(id).subscribe(result => {
          this.refresh();
          this.messageService.success("删除成功！");
        })
    });
  }

  submitSearch() {
    this.page = 1;
    this.refresh();
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      let user = new User();
      user.userName = this.editForm.value['userName'];
      user.password = this.editForm.value['password'];
      user.name = this.editForm.value['name'];
      user.phoneNumber = this.editForm.value['phoneNumber'].toString();
      user.roleNames = (this.editForm.value['roleNames']).filter(item => item !== '').join(',');
      user.sex = this.editForm.value['sex'];
      user.isActive = this.editForm.value['isActive'];
      if (this.editedUser.id) {
        user.id = this.editedUser.id;
        this.userService.update(user).subscribe(result => {
          this.messageService.success("更新成功！");
          this.tplModal.close();
          this.refresh();
        });
      } else {
        this.userService.add(user).subscribe(result => {
          this.messageService.success("添加成功！");
          this.tplModal.close();
          this.refresh();
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
