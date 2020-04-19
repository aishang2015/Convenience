import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/constants/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  get() {
    return this.httpClient.get(this.uriConstant.DepartmentUri);
  }

  delete(id) {
    return this.httpClient.delete(`${this.uriConstant.DepartmentUri}?id=${id}`);
  }

  update(menu) {
    return this.httpClient.patch(this.uriConstant.DepartmentUri, menu);
  }

  add(menu) {
    return this.httpClient.post(this.uriConstant.DepartmentUri, menu);
  }
}
