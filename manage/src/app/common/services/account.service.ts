import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonConstant } from '../constants/common-constant';
import { StorageService } from 'src/app/core/services/storage.service';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient) { }

  login(userName, password):Observable<any> {
    return this.httpClient.post(CommonConstant.LoginUri, { "UserName": userName, "Password": password });

  }


}
