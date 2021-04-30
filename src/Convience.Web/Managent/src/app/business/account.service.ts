import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from '../configs/uri-config';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }

  login(userName, password, captchaKey, captchaValue) {
    return this.httpClient.post<LoginResult>(this.uriConstant.LoginUri,
      { "UserName": userName, "Password": password, "CaptchaKey": captchaKey, "CaptchaValue": captchaValue });
  }

  modifyPassword(oldPassword, newPassword) {
    return this.httpClient.post(this.uriConstant.ModifySelfPasswordUri, { "OldPassword": oldPassword, "NewPassword": newPassword });
  }

  getCaptcha() {
    return this.httpClient.get<CaptchaResult>(this.uriConstant.CaptchaUri);
  }
}

export interface CaptchaResult {
  captchaKey: string;
  captchaData: string;
}

export interface LoginResult {
  name: string;
  avatar: string;
  token: string;
  identification: string;
  routes: string;
}
