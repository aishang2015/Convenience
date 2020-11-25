import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from '../configs/uri-config';

@Injectable({
  providedIn: 'root'
})
export class OperateLogService {


  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }

  // 获取日志配置
  getSettings(page, size, sort, order) {
    let uri = `${this.uriConstant.OperateLogSettingUri}?page=${page}&&size=${size}&&sort=${sort}&&order=${order}`;
    return this.httpClient.get(uri);
  }

  // 更新日志配置
  updateSetting(data) {
    let uri = this.uriConstant.OperateLogSettingUri;
    return this.httpClient.patch(uri, data);
  }

  // 获取日志内容
  getDetails(page, size, sort, order) {
    let uri = `${this.uriConstant.OperateLogDetailUri}?page=${page}&&size=${size}&&sort=${sort}&&order=${order}`;
    return this.httpClient.get(uri);
  }

}
