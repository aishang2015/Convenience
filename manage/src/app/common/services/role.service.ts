import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../constants/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private httpClient: HttpClient) { }

  getRole(name, page, size) {
    var uri = UriConstant.RoleUri;
    if (name) {
      uri += `?name=${name}&&page=${page}&&size=${size}`;
    } else {
      uri += `?page=${page}&&size=${size}`;
    }
    return this.httpClient.get(uri);
  }

  deleteRole(name) {
    return this.httpClient.delete(`${UriConstant.RoleUri}?name=${name}`);
  }

  addRole(role) {
    return this.httpClient.post(UriConstant.RoleUri, role);
  }

  updateRole(role) {
    return this.httpClient.patch(UriConstant.RoleUri, role);
  };
}
