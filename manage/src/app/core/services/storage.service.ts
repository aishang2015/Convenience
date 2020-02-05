import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  constructor() { }

  set userToken(value) {
    localStorage.setItem('userToken', value);
  }

  get userToken() {
    return localStorage.getItem('userToken');
  }

  removeUserToken() {
    localStorage.removeItem('userToken');
  }

  hasUserToken(): boolean {
    return this.userToken != null;
  }

}
