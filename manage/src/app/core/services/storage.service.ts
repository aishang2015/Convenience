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
    console.log(expirationDate);
    localStorage.setItem('userTokenExpiration', expirationDate.toString());
  }

  get userToken() {
    return localStorage.getItem('userToken');
  }

  get tokenExpiration(){
    return localStorage.getItem('userTokenExpiration');
  }

  removeUserToken() {
    localStorage.removeItem('userToken');
  }

  hasUserToken(): boolean {
    return this.userToken != null;
  }

}
