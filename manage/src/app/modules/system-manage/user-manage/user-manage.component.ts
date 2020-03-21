import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { User } from '../model/user';
import { UserService } from 'src/app/common/services/user.service';
import { NzModalService, NzModalRef } from 'ng-zorro-antd';
import { RoleService } from 'src/app/common/services/role.service';

@Component({
  selector: 'app-user-manage',
  templateUrl: './user-manage.component.html',
  styleUrls: ['./user-manage.component.scss']
})
export class UserManageComponent implements OnInit {

  searchForm: FormGroup;
  editForm: FormGroup;
  isNewUser: boolean;

  tplModal: NzModalRef;

  page: number = 1;
  size: number = 10;
  total: number = 0;

  data: User[] = [];
  roleNames: string[] = [];

  constructor(private formBuilder: FormBuilder,
    private userService: UserService,
    private roleService: RoleService,
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
        this.size = result['count'];
      });
  }

  add(title: TemplateRef<{}>, content: TemplateRef<{}>) {
    this.isNewUser = true;
    this.editForm = this.formBuilder.group({
      userName: [null, [Validators.required, Validators.maxLength(15)]],
      password: [null, [Validators.required, Validators.maxLength(30)]],
      name: [null, [Validators.required, Validators.maxLength(10)]],
      phoneNumber: [null, [Validators.pattern('^1(3|4|5|7|8)[0-9]{9}$')]],
      roleNames: [null],
      sex: ['0'],
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
    this.isNewUser = false;
    this.editForm = this.formBuilder.group({
      userName: [null, [Validators.required, Validators.maxLength(15)]],
      password: [null, [Validators.maxLength(30)]],
      name: [null, [Validators.required, Validators.maxLength(10)]],
      phoneNumber: [null, [Validators.pattern('^1(3|4|5|7|8)[0-9]{9}$')]],
      roleNames: [null],
      sex: ['0'],
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


  remove(id: string) { }

  submitSearch() {
    this.page = 1;
    this.refresh();
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
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
