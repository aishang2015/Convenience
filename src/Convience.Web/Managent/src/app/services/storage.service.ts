import { Injectable } from '@angular/core';
import { JwtHelperService } from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  jwtService = new JwtHelperService();

  constructor() { }

  set userToken(value) {
    localStorage.setItem('userToken', value);
    let expirationDate = this.jwtService.getTokenExpirationDate(value);
    let decodeToken = this.jwtService.decodeToken(value);
    this.UserRoles = decodeToken['userroleids'];
    this.userName = decodeToken['username'];
    localStorage.setItem('userTokenExpiration', expirationDate.toString());
  }

  get userToken() {
    return localStorage.getItem('userToken');
  }

  get tokenExpiration() {
    return localStorage.getItem('userTokenExpiration');
  }

  get IsTokenExpire() {
    let now = new Date();
    let expire = new Date(this.tokenExpiration);
    return now > expire;
  }

  get userName() {
    return localStorage.getItem("username");
  }
  set userName(value) {
    localStorage.setItem("username", value);
  }

  removeUserToken() {
    localStorage.removeItem('userToken');
  }

  hasUserToken(): boolean {
    return this.userToken != null;
  }

  get Name() {
    return localStorage.getItem("name");
  }
  set Name(value) {
    localStorage.setItem("name", value);
  }

  get UserRoles() {
    return localStorage.getItem("userroles");
  }
  set UserRoles(value) {
    localStorage.setItem("userroles", value);
  }

  get Avatar() {
    return localStorage.getItem("avatar");
  }
  set Avatar(value) {
    localStorage.setItem("avatar", value);
  }

  get Identifycation() {
    return localStorage.getItem("identifycation");
  }
  set Identifycation(value) {
    localStorage.setItem("identifycation", value);
  }

  get Route() {
    return localStorage.getItem("route");
  }
  set Route(value) {
    localStorage.setItem("route", value);
  }

  clearTinymceCache() {
    for (let i = 0; i < localStorage.length; i++) {
      let key = localStorage.key(i);
      if (key.startsWith('tinymce-autosave') || key.startsWith('tinymce-url')) {
        localStorage.removeItem(key);
      }
    }
  }
}
