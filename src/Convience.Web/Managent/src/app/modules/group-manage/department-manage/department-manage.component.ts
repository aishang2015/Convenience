import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NzModalRef, NzMessageService, NzModalService } from 'ng-zorro-antd';
import { Department } from '../model/department';

@Component({
  selector: 'app-department-manage',
  templateUrl: './department-manage.component.html',
  styleUrls: ['./department-manage.component.scss']
})
export class DepartmentManageComponent implements OnInit {

  data: Department[] = [];

  size: number = 10;
  page: number = 1;
  total: number = 0;

  @ViewChild('editTitleTpl', { static: true })
  editTitleTpl;

  @ViewChild('addTitleTpl', { static: true })
  addTitleTpl;

  @ViewChild('contentTpl', { static: true })
  contentTpl;

  editForm: FormGroup = new FormGroup({});

  modal: NzModalRef;

  constructor(
    private messageService: NzMessageService,
    private modalService: NzModalService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
  }

  add() {

    this.modal = this.modalService.create({
      nzTitle: this.addTitleTpl,
      nzContent: this.contentTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });

  }

  refresh() { }

  edit(id) { }

  remove(id) {
    this.modalService.confirm({
      nzTitle: '是否删除该?',
      nzContent: null,
      nzOnOk: () => { },
    });
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {

    }
  }

  cancel() {
    this.modal.close();
  }

  getUpperDepartment(id){

  }

}
