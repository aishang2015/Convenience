import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { OperateLogService } from '../../../business/operate-log.service';

@Component({
  selector: 'app-operate-log-manage',
  templateUrl: './operate-log-manage.component.html',
  styleUrls: ['./operate-log-manage.component.less']
})
export class OperateLogManageComponent implements OnInit {

  public data: any[] = [];

  private _searchObject: any = {};

  public editForm: FormGroup = new FormGroup({});

  public searchForm: FormGroup = new FormGroup({});

  // 表格排序功能
  private _sortArray: string[] = [];

  private _orderArray: string[] = [];

  private _nzModal;

  @ViewChild("editTpl", { static: true })
  editTpl;

  public page = 1;
  public size = 10;
  public total = 0;

  constructor(private _operateLogService: OperateLogService,
    private _modalService: NzModalService,
    private _formBuilder: FormBuilder,
    private _messageService: NzMessageService) { }

  ngOnInit(): void {
    this.initSearchForm();
    this.initData();
  }

  // 初始化搜索表单
  public initSearchForm() {
    this.searchForm = this._formBuilder.group({
      "module": [null],
      "submodule": [null]
    });
  }

  // 初始化数据
  public initData() {
    this._operateLogService.getSettings(this.page, this.size, this._sortArray, this._orderArray, this._searchObject).subscribe((result: any) => {
      this.data = result.data;
      this.total = result.count
    })
  }

  public pageChange() {
    this.initData();
  }

  public sizeChange() {
    this.page = 1;
    this.initData();
  }

  public edit(data) {

    this.editForm = this._formBuilder.group({
      "id": [data.id],
      "savetime": [data.saveTime, [Validators.required]],
      "isRecord": [data.isRecord]
    })

    this._nzModal = this._modalService.create({
      nzTitle: "修改日志配置",
      nzContent: this.editTpl,
      nzFooter: null
    });
  }

  public submitSearch() {
    this._searchObject.module = this.searchForm.value["module"];
    this._searchObject.submodule = this.searchForm.value["submodule"];
    this.initData();
  }

  public submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      let data: any = {};
      data.Id = this.editForm.value["id"];
      data.SaveTime = this.editForm.value["savetime"];
      data.IsRecord = this.editForm.value["isRecord"];
      this._operateLogService.updateSetting(data).subscribe(result => {
        this._messageService.success("操作成功！");
        this._nzModal.close();
        this.initData();
      })
    }
  }

  public cancelEdit() {
    this._nzModal.close();
  }

  // 排序发生变化
  public onQueryParamsChange(params: NzTableQueryParams) {
    let currentSort = params.sort.filter(s => s.value != null);

    // 移除了排序字段
    if (this._sortArray.length > currentSort.length) {
      let removedField = this._sortArray.find(f => currentSort.find(c => c.key == f) == null);
      let removeIndex = this._sortArray.findIndex(f => f == removedField);

      // 移除元素
      this._sortArray.splice(removeIndex, 1);
      this._orderArray.splice(removeIndex, 1);
    } else if (this._sortArray.length < currentSort.length) {

      // 添加了排序字段
      let newField = currentSort.find(c => this._sortArray.find(f => f == c.key) == null);
      this._sortArray.push(newField.key);
      this._orderArray.push(newField.value);
    } else {
      for (let s of currentSort) {
        let index = this._sortArray.findIndex(f => f == s.key);
        this._orderArray[index] = s.value;
      }
    }
    this.initData();
  }
}
