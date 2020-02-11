import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonConstant } from '../constants/common-constant';
import { StorageService } from 'src/app/core/services/storage.service';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient,
    private storage: StorageService,
    private router: Router,
    private messageService: NzMessageService) { }

  login(userName, password) {
    this.httpClient.post(CommonConstant.LoginUri, { "UserName": userName, "Password": password })
      .subscribe(
        result => {
          this.storage.userToken = result["Token"];
          this.router.navigate(['/dashboard']);
        },
        error => {
          this.messageService.error(error['error'][''][0]);
        }
      )
  }


}
