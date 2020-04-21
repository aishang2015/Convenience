import { Injectable } from '@angular/core';
import { UriConstant } from '../core/constants/uri-constant';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  get(id) {
    return this.httpClient.get(`${this.uriConstant.EmployeeUri}?id=${id}`);
  }

  getEmployees(page, size, phoneNumber, name, departmentId, positionId) {

    let uri = `${this.uriConstant.EmployeeUri}/list?page=${page}&&size=${size}`;
    uri += phoneNumber ? `&&phoneNumber=${phoneNumber}` : '';
    uri += name ? `&&name=${name}` : '';
    uri += departmentId ? `&&departmentId=${departmentId}` : '';
    uri += positionId ? `&&positionId=${positionId}` : '';
    return this.httpClient.get(uri);
  }

  updateEmployee(data) {
    return this.httpClient.patch(this.uriConstant.EmployeeUri, data);
  }
}
