import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from '../configs/uri-config';

@Injectable({
  providedIn: 'root'
})
export class TenantService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }

  getTenant(id) {
    return this.httpClient.get(`${this.uriConstant.TenantUri}?id=${id}`);
  }

  getTenants(page, size, name, dataBaseType, sortKey, isDesc) {
    var uri = `${this.uriConstant.TenantUri}/list?page=${page}&&size=${size}`;
    uri += name ? `&&name=${name}` : '';
    uri += dataBaseType || dataBaseType == 0 ? `&&dataBaseType=${dataBaseType}` : '';
    uri += sortKey ? `&&sortKey=${sortKey}` : '';
    uri += isDesc ? `&&isDesc=${isDesc}` : '';
    return this.httpClient.get(uri);
  }

  delete(id: String) {
    return this.httpClient.delete(`${this.uriConstant.TenantUri}?id=${id}`);
  }

  update(tenant) {
    return this.httpClient.patch(this.uriConstant.TenantUri, tenant);
  }

  add(tenant) {
    return this.httpClient.post(this.uriConstant.TenantUri, tenant);
  }
}
