import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Department } from '../model/department';
import { debounceTime } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { UserService } from 'src/app/business/system-manage/user.service';
import { DepartmentService } from 'src/app/business/group-manage/department.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-department-manage',
  templateUrl: './department-manage.component.html',
  styleUrls: ['./department-manage.component.less']
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

  @ViewChild('tree', { static: true })
  tree: any;

  editForm: FormGroup = new FormGroup({});

  modal: NzModalRef;

  isLoading = false;

  searchChange$ = new BehaviorSubject('');

  userDicList = [];

  constructor(
    private _messageService: NzMessageService,
    private _modalService: NzModalService,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _departmentService: DepartmentService) { }

  ngOnInit(): void {
    this.searchChange$
      .asObservable()
      .pipe(debounceTime(800)).subscribe(value => {
        this._userService.getUserDic(value).subscribe((result: any) => {
          this.userDicList = result;
          this.isLoading = false;
        });
      });
  }

  add() {
    this.editForm = this._formBuilder.group({
      id: [null],
      upDepartment: [this.tree.selectedNode?.key],
      name: [null, [Validators.required, Validators.maxLength(15)]],
      email: [null, [Validators.email, Validators.maxLength(50)]],
      telephone: [null, [Validators.maxLength(20)]],
      leaderid: [null],
      sort: [null, Validators.required]
    });
    this.modal = this._modalService.create({
      nzTitle: this.addTitleTpl,
      nzContent: this.contentTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  refresh() {
    this.tree.initNodes();
  }

  edit(id) {
    this.searchChange$.next(this.data.find(d => d.id == id).leaderName);
    this._departmentService.get(id).subscribe((result: Department) => {
      let upId = this.tree.data.find(d => d.id == result.id)?.upId;
      this.editForm = this._formBuilder.group({
        id: [result.id],
        upDepartment: [{ value: Number(upId), disabled: true }],
        name: [result.name, [Validators.required, Validators.maxLength(15)]],
        email: [result.email, [Validators.email, Validators.maxLength(50)]],
        telephone: [result.telephone, [Validators.maxLength(20)]],
        leaderid: [result.leaderId?.toString()],
        sort: [result.sort, Validators.required]
      });

      this.modal = this._modalService.create({
        nzTitle: this.addTitleTpl,
        nzContent: this.contentTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });

  }

  remove(id) {
    this._modalService.confirm({
      nzTitle: '是否删除该部门?',
      nzContent: null,
      nzOnOk: () => {
        this._departmentService.delete(id).subscribe(reuslt => {
          this._messageService.success("删除成功！");
          this.refresh();
        });
      },
    });
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      let d = new Department();
      d.id = this.editForm.value['id'];
      d.upId = this.editForm.value['upDepartment'];
      d.name = this.editForm.value['name'];
      d.email = this.editForm.value['email'];
      d.telephone = this.editForm.value['telephone'];
      d.leaderId = Number(this.editForm.value['leaderid']);
      d.sort = this.editForm.value['sort'];
      if (d.id) {
        this._departmentService.update(d).subscribe(reuslt => {
          this._messageService.success("修改成功！");
          this.refresh();
          this.modal.close();
        });
      } else {
        this._departmentService.add(d).subscribe(reuslt => {
          this._messageService.success("添加成功！");
          this.refresh();
          this.modal.close();
        });
      }
    }
  }

  cancel() {
    this.modal.close();
  }

  getUpperDepartment(id) {
    return this.tree.data.find(d => d.id == id)?.name;
  }

  onSearch(value: string): void {
    this.isLoading = true;
    this.searchChange$.next(value);
  }

  selectedChanged(array: Department[]) {
    this.data = array;
  }

}
