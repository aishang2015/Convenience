import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/constants/uri-constant';
import { url } from 'inspector';
import { User } from 'src/app/modules/system-manage/model/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  getUser(id) {
    return this.httpClient.get(`${this.uriConstant.UserUri}?id=${id}`);
  }

  getUsers(page, size, userName, phoneNumber, name, roleid) {
    let uri = `${this.uriConstant.UserUri}/list?page=${page}&&size=${size}`;
    uri += userName ? `&&userName=${userName}` : '';
    uri += phoneNumber ? `&&phoneNumber=${phoneNumber}` : '';
    uri += name ? `&&name=${name}` : '';
    uri += roleid ? `&&roleid=${roleid}` : '';
    return this.httpClient.get(uri);
  }

  getUserDic(name) {
    let uri = `${this.uriConstant.UserUri}/dic?name=${name}`;
    return this.httpClient.get(uri);
  }

  delete(id: String) {
    return this.httpClient.delete(`${this.uriConstant.UserUri}?id=${id}`);
  }

  update(user: User) {
    return this.httpClient.patch(this.uriConstant.UserUri, user);
  }

  add(user: User) {
    return this.httpClient.post(this.uriConstant.UserUri, user);
  }

}
