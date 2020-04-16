import { Component, OnInit, ViewChild } from '@angular/core';
import { StorageService } from 'src/app/core/services/storage.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { NzModalService, NzModalRef, NzMessageService } from 'ng-zorro-antd';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

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

  constructor(private storageService: StorageService, private router: Router,
    private fb: FormBuilder,
    private modalService: NzModalService,
    private messageService: NzMessageService,
    private accountService: AccountService) { }

  ngOnInit() {
    this.name = this.storageService.Name;
    this.avatar = this.storageService.Avatar;
  }

  logout() {
    this.storageService.removeUserToken();
    this.router.navigate(['/account/login']);
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
    this.modifyForm = this.fb.group({
      // key: value,validators,asyncvalidators,updateOn
      userName: [{ value: this.storageService.userName, disabled: true }],
      oldPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required, this.equalValidator]]
    });
    this.modalRef = this.modalService.create({
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
      this.accountService.modifyPassword(this.modifyForm.controls['oldPassword'].value, this.modifyForm.controls['newPassword'].value)
        .subscribe(
          result => {
            this.modifyForm.reset();
            this.messageService.success("密码修改成功！");
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
