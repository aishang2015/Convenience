import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from '../configs/uri-config';
import { DatePipe } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class OperateLogService {


  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig,    
    private _datePipe: DatePipe) { }

  // 获取日志配置
  getSettings(page, size, sort, order, searchObj) {
    let uri = `${this.uriConstant.OperateLogSettingUri}?page=${page}&&size=${size}&&sort=${sort}&&order=${order}`;
    uri += searchObj.module ? `&&module=${searchObj.module}` : '';
    uri += searchObj.submodule ? `&&submodule=${searchObj.submodule}` : '';
    return this.httpClient.get(uri);
  }

  // 更新日志配置
  updateSetting(data) {
    let uri = this.uriConstant.OperateLogSettingUri;
    return this.httpClient.patch(uri, data);
  }

  // 获取日志内容
  getDetails(page, size, sort, order, searchObj) {
    let uri = `${this.uriConstant.OperateLogDetailUri}?page=${page}&&size=${size}&&sort=${sort}&&order=${order}`;
    uri += searchObj.module ? `&&module=${searchObj.module}` : '';
    uri += searchObj.submodule ? `&&submodule=${searchObj.submodule}` : '';
    uri += searchObj.operator ? `&&operator=${searchObj.operator}` : '';
    uri += searchObj.operateAt && searchObj.operateAt[0] ? `&&startAt=${this._datePipe.transform(searchObj.operateAt[0], 'yyyy-MM-dd HH:mm:ss')}` : '';
    uri += searchObj.operateAt && searchObj.operateAt[1] ? `&&endAt=${this._datePipe.transform(searchObj.operateAt[1], 'yyyy-MM-dd HH:mm:ss')}` : '';
    return this.httpClient.get(uri);
  }

}
