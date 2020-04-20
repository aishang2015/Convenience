import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NzModalRef, NzMessageService, NzModalService } from 'ng-zorro-antd';
import { Department } from '../model/department';
import { DepartmentTreeComponent } from '../department-tree/department-tree.component';
import { debounce, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { DepartmentService } from 'src/app/services/department.service';
import { timingSafeEqual } from 'crypto';

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

  currentId?: number = null;

  @ViewChild('editTitleTpl', { static: true })
  editTitleTpl;

  @ViewChild('addTitleTpl', { static: true })
  addTitleTpl;

  @ViewChild('contentTpl', { static: true })
  contentTpl;

  @ViewChild('tree', { static: true })
  tree: DepartmentTreeComponent;

  editForm: FormGroup = new FormGroup({});

  modal: NzModalRef;

  isLoading = false;

  searchChange$ = new BehaviorSubject('');

  userDicList = [];

  constructor(
    private messageService: NzMessageService,
    private modalService: NzModalService,
    private formBuilder: FormBuilder,
    private userService: UserService,
    private departmentService: DepartmentService) { }

  ngOnInit(): void {
    this.searchChange$
      .asObservable()
      .pipe(debounceTime(800)).subscribe(value => {
        this.userService.getUserDic(value).subscribe((result: any) => {
          this.userDicList = result;
          this.isLoading = false;
        });
      });
  }

  add() {
    this.editForm = this.formBuilder.group({
      upDepartment: [{ value: this.tree.selectedNode?.title, disabled: true }],
      name: [null, [Validators.required, Validators.maxLength(15)]],
      email: [null, [Validators.email, Validators.maxLength(50)]],
      telephone: [null, [Validators.maxLength(20)]],
      leaderid: [null],
      sort: [null, Validators.required]
    });
    this.modal = this.modalService.create({
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
    this.departmentService.get(id).subscribe((result: Department) => {
      this.currentId = result.id;
      this.editForm = this.formBuilder.group({
        upDepartment: [{ value: this.getUpperDepartment(result.upId), disabled: true }],
        name: [result.name, [Validators.required, Validators.maxLength(15)]],
        email: [result.email, [Validators.email, Validators.maxLength(50)]],
        telephone: [result.telephone, [Validators.maxLength(20)]],
        leaderid: [result.leaderId?.toString()],
        sort: [result.sort, Validators.required]
      });

      this.modal = this.modalService.create({
        nzTitle: this.addTitleTpl,
        nzContent: this.contentTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });

  }

  remove(id) {
    this.modalService.confirm({
      nzTitle: '是否删除该部门?',
      nzContent: null,
      nzOnOk: () => {
        this.departmentService.delete(id).subscribe(reuslt => {
          this.messageService.success("删除成功！");
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
      d.upId = this.tree.selectedNode?.key?.toString();
      d.name = this.editForm.value['name'];
      d.email = this.editForm.value['email'];
      d.telephone = this.editForm.value['telephone'];
      d.leaderId = Number(this.editForm.value['leaderid']);
      d.sort = this.editForm.value['sort'];
      if (this.currentId) {
        d.id = this.currentId;
        this.departmentService.update(d).subscribe(reuslt => {
          this.messageService.success("修改成功！");
          this.refresh();
          this.modal.close();
        });
      } else {
        this.departmentService.add(d).subscribe(reuslt => {
          this.messageService.success("添加成功！");
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
