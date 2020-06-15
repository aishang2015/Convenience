import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/constants/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  get(id) {
    return this.httpClient.get(`${this.uriConstant.DepartmentUri}?id=${id}`);
  }

  getAll() {
    return this.httpClient.get(`${this.uriConstant.DepartmentUri}/all`);
  }

  delete(id) {
    return this.httpClient.delete(`${this.uriConstant.DepartmentUri}?id=${id}`);
  }

  update(department) {
    return this.httpClient.patch(this.uriConstant.DepartmentUri, department);
  }

  add(department) {
    return this.httpClient.post(this.uriConstant.DepartmentUri, department);
  }

  getDic(name) {
    let uri = `${this.uriConstant.DepartmentUri}/dic?name=${name}`;
    return this.httpClient.get(uri);
  }
}
