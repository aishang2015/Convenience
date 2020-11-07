import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Position } from '../model/position';
import { Employee } from '../model/employee';
import { EmployeeService } from 'src/app/business/employee.service';
import { PositionService } from 'src/app/business/position.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { NzTreeNodeOptions } from 'ng-zorro-antd/tree';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss']
})
export class EmployeeComponent implements OnInit {

  data: Employee[] = [];

  size: number = 10;
  page: number = 1;
  total: number = 0;

  currentId?: number = null;

  @ViewChild('editTitleTpl', { static: true })
  editTitleTpl;

  @ViewChild('contentTpl', { static: true })
  contentTpl;

  editForm: FormGroup = new FormGroup({});
  searchForm: FormGroup = new FormGroup({});

  modal: NzModalRef;

  departmentNode: NzTreeNodeOptions[] = [];
  positionOptions: Position[] = [];

  selectedDepartmentKey: string = '';

  // store search parameters
  private _searchObject: any = {};

  constructor(
    private _messageService: NzMessageService,
    private _modalService: NzModalService,
    private _formBuilder: FormBuilder,
    private _employeeService: EmployeeService,
    private _positionService: PositionService) { }

  ngOnInit(): void {
    this._positionService.getAll().subscribe((result: any) => {
      this.positionOptions = result;
    });
    this.resetSearchForm();
    this.refresh();
  }

  refresh() {
    this._employeeService.getEmployees(this.page, this.size,
      this._searchObject?.phoneNumber,
      this._searchObject?.name,
      this.selectedDepartmentKey,
      this._searchObject?.position)
      .subscribe((result: any) => {
        this.data = result.data;
        this.total = result.count;
      });
  }

  edit(id) {
    this._employeeService.get(id).subscribe((result: any) => {
      this.currentId = id;
      this.editForm = this._formBuilder.group({
        name: [{ value: result['name'], disabled: true }],
        department: [Number(result['departmentId'])],
        positions: [result['positionIds']?.split(',')]
      });
      this.modal = this._modalService.create({
        nzTitle: this.editTitleTpl,
        nzContent: this.contentTpl,
        nzFooter: null,
      })
    });
  }

  // reset the search form content 
  resetSearchForm() {
    this.searchForm = this._formBuilder.group({
      phoneNumber: [null],
      name: [null],
      position: [null]
    });
  }

  submitSearch() {
    this._searchObject.phoneNumber = this.searchForm.value['phoneNumber'];
    this._searchObject.name = this.searchForm.value['name'];
    this._searchObject.position = this.searchForm.value['position'];
    this.refresh();
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      let employee = new Employee()
      employee.id = this.currentId;
      employee.departmentId = this.editForm.value['department']?.toString();
      employee.positionIds = this.editForm.value['positions'].join(',');
      this._employeeService.updateEmployee(employee).subscribe(() => {
        this._messageService.success("修改成功！");
        this.refresh();
        this.modal.close();
      });
    }
  }

  cancel() {
    this.modal.close();
  }

  pageChange() {
    this.refresh();
  }

  sizeChange() {
    this.page = 1;
    this.refresh();
  }

  getImgUrl(name) {
    return `/assets/avatars/${name}.png`;
  }

  getPositionName(id) {
    return this.positionOptions.find(p => p.id.toString() == id)?.name;
  }

  nodeChecked(key) {
    this.selectedDepartmentKey = key;
    this.refresh();
  }

  loadedData(nodes) {
    this.departmentNode = nodes;
  }


}
