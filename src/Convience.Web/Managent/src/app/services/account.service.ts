import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/constants/uri-constant';
import { StorageService } from 'src/app/core/services/storage.service';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  login(userName, password): Observable<any> {
    return this.httpClient.post(this.uriConstant.LoginUri, { "UserName": userName, "Password": password });
  }

  modifyPassword(oldPassword, newPassword) {
    return this.httpClient.post(this.uriConstant.ModifySelfPasswordUri, { "OldPassword": oldPassword, "NewPassword": newPassword });
  }


}
