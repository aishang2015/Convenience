import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalService } from 'ng-zorro-antd/modal';
import { LoginLogService } from 'src/app/business/login-log.service';

@Component({
  selector: 'app-login-log-view',
  templateUrl: './login-log-view.component.html',
  styleUrls: ['./login-log-view.component.less']
})
export class LoginLogViewComponent implements OnInit {

  public data: any[] = [];

  private _searchObject: any = {};

  public searchForm: FormGroup = new FormGroup({});

  public settingForm: FormGroup = new FormGroup({});

  // 表格排序功能

  private _nzModal;

  public page = 1;
  public size = 10;
  public total = 0;

  @ViewChild("settingTpl", { static: true })
  settingTpl;

  constructor(private _modalService: NzModalService,
    private _formBuilder: FormBuilder,
    private _loginService: LoginLogService,
    private _messageService: NzMessageService) { }

  ngOnInit(): void {
    this.initSearchForm();
    this.initData();
  }

  initData() {
    this._loginService.getDetails(this.page, this.size, this._searchObject).subscribe((result: any) => {
      this.data = result.data;
      this.total = result.count;
    });
  }

  // 初始化搜索表单
  public initSearchForm() {
    this.searchForm = this._formBuilder.group({
      "account": [null]
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
    this._searchObject = {};
    this._searchObject.account = this.searchForm.value["account"];
    this.initData();
  }

  public setLoginLogSetting() {
    this._loginService.getSettings().subscribe((result: any) => {
      this.settingForm = this._formBuilder.group({
        "savetime": [result.saveTime, [Validators.required]],
      });
      this._nzModal = this._modalService.create({
        nzContent: this.settingTpl,
        nzTitle: "设置登录日志",
        nzFooter: null,
      });
    });
  }

  public submitSetting() {
    for (const i in this.settingForm.controls) {
      this.settingForm.controls[i].markAsDirty();
      this.settingForm.controls[i].updateValueAndValidity();
    }
    if (this.settingForm.valid) {
      this._loginService.updateSetting(this.settingForm.value['savetime']).subscribe(result => {
        this._messageService.success("保存成功！");
        this._nzModal.close();
      });
    }
  }

  public cancelEdit() {
    this._nzModal.close();
  }

}
