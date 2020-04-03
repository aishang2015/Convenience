import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../constants/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  getRole(id) {
    return this.httpClient.get(`${this.uriConstant.RoleUri}?id=${id}`);
  }

  getRoles(name, page, size) {
    var uri = `${this.uriConstant.RoleUri}/list`;
    if (name) {
      uri += `?name=${name}&&page=${page}&&size=${size}`;
    } else {
      uri += `?page=${page}&&size=${size}`;
    }
    return this.httpClient.get(uri);
  }

  deleteRole(name) {
    return this.httpClient.delete(`${this.uriConstant.RoleUri}?name=${name}`);
  }

  addRole(role) {
    return this.httpClient.post(this.uriConstant.RoleUri, role);
  }

  updateRole(role) {
    return this.httpClient.patch(this.uriConstant.RoleUri, role);
  };

  getRoleList() {
    return this.httpClient.get(this.uriConstant.RoleUri + '/nameList');
  }
}
