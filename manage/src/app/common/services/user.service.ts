import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../constants/uri-constant';
import { url } from 'inspector';
import { User } from 'src/app/modules/system-manage/model/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient) { }

  getUser(id) {
    return this.httpClient.get(`${UriConstant.UserUri}?id=${id}`);
  }

  getUsers(page, size, userName, phoneNumber, name, roleName) {
    var uri = `${UriConstant.UserUri}/list?page=${page}&&size=${size}`;
    uri += userName ? `&&userName=${userName}` : '';
    uri += phoneNumber ? `&&phoneNumber=${phoneNumber}` : '';
    uri += name ? `&&name=${name}` : '';
    uri += roleName ? `&&roleName=${roleName}` : '';
    return this.httpClient.get(uri);
  }

  delete(id: String) {
    return this.httpClient.delete(`${UriConstant.UserUri}?id=${id}`);
  }

  update(user: User) {
    return this.httpClient.patch(UriConstant.UserUri, user);
  }

  add(user: User) {
    return this.httpClient.post(UriConstant.UserUri, user);
  }

}
