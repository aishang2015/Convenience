import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from '../configs/uri-config';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }

  login(userName, password, captchaKey, captchaValue): Observable<any> {
    return this.httpClient.post(this.uriConstant.LoginUri,
      { "UserName": userName, "Password": password, "CaptchaKey": captchaKey, "CaptchaValue": captchaValue });
  }

  modifyPassword(oldPassword, newPassword) {
    return this.httpClient.post(this.uriConstant.ModifySelfPasswordUri, { "OldPassword": oldPassword, "NewPassword": newPassword });
  }

  getCaptcha() {
    return this.httpClient.get(this.uriConstant.CaptchaUri);
  }


}
