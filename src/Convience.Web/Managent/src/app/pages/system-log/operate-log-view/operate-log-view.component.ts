import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { OperateLogService } from 'src/app/business/operate-log.service';

@Component({
  selector: 'app-operate-log-view',
  templateUrl: './operate-log-view.component.html',
  styleUrls: ['./operate-log-view.component.less']
})
export class OperateLogViewComponent implements OnInit {

  public data: any[] = [];

  private _searchObject: any = {};

  public editForm: FormGroup = new FormGroup({});

  public searchForm: FormGroup = new FormGroup({});

  // 表格排序功能
  private _sortArray: string[] = [];

  private _orderArray: string[] = [];

  private _nzModal;

  public page = 1;
  public size = 10;
  public total = 0;

  @ViewChild("settingTpl", { static: true })
  settingTpl;

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
      "submodule": [null],
      "operator": [null],
      "operateAt": [null],
    });
  }

  // 初始化数据
  public initData() {
    this._operateLogService.getDetails(this.page, this.size, this._sortArray, this._orderArray, this._searchObject).subscribe((result: any) => {
      this.total = result.count;
      this.data = result.data;
    });
  }

  public pageChange() {
    this.initData();
  }

  public sizeChange() {
    this.page = 1;
    this.initData();
  }

  public submitSearch() {
    this._searchObject.module = this.searchForm.value["module"];
    this._searchObject.submodule = this.searchForm.value["submodule"];
    this._searchObject.operator = this.searchForm.value["operator"];
    this._searchObject.operateAt = this.searchForm.value["operateAt"];
    this.initData();
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

  // 打开配置页面
  openOperateLogSettingPage() {
    this._modalService.create({
      nzContent: this.settingTpl,
      nzTitle: null,
      nzFooter: null,
      nzWidth: '1400px'
    })
  }
}
