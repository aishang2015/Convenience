import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UriConfig } from '../configs/uri-config';

@Injectable({
  providedIn: 'root'
})
export class LoginLogService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig,
    private _datePipe: DatePipe) { }

  // 获取日志配置
  getSettings() {
    let uri = `${this.uriConstant.LoginLogUri}/setting`;
    return this.httpClient.get(uri);
  }

  // 更新日志配置
  updateSetting(time) {
    let uri = `${this.uriConstant.LoginLogUri}/setting`;
    return this.httpClient.post(uri, {
      saveTime: time
    });
  }

  // 获取日志内容
  getDetails(page, size, searchObj) {
    let uri = `${this.uriConstant.LoginLogUri}?page=${page}&&size=${size}`;
    uri += searchObj.account ? `&&account=${searchObj.account}` : '';
    return this.httpClient.get(uri);
  }
}
