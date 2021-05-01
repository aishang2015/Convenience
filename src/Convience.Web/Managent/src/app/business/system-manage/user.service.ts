import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from 'src/app/configs/uri-config';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }

  getUser(id) {
    return this.httpClient.get(`${this.uriConstant.UserUri}?id=${id}`);
  }

  getUsers(page, size, department, userName, phoneNumber, name, roleid, position) {
    let uri = `${this.uriConstant.UserUri}/list?page=${page}&&size=${size}`;
    uri += department ? `&&department=${department}` : '';
    uri += userName ? `&&userName=${userName}` : '';
    uri += phoneNumber ? `&&phoneNumber=${phoneNumber}` : '';
    uri += name ? `&&name=${name}` : '';
    uri += roleid ? `&&roleid=${roleid}` : '';
    uri += position ? `&&position=${position}` : '';
    return this.httpClient.get(uri);
  }

  getUserDic(name) {
    let uri = `${this.uriConstant.UserUri}/dic?name=${name}`;
    return this.httpClient.get(uri);
  }

  delete(id: String) {
    return this.httpClient.delete(`${this.uriConstant.UserUri}?id=${id}`);
  }

  update(user) {
    return this.httpClient.patch(this.uriConstant.UserUri, user);
  }

  add(user) {
    return this.httpClient.post(this.uriConstant.UserUri, user);
  }

  setPwd(model: UserPassword) {
    return this.httpClient.post<UserPassword>(`${this.uriConstant.UserUri}/password`, model);
  }
}

export interface UserPassword {
  id?: number;
  password?: string;
}
