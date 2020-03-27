import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../constants/uri-constant';
import { Tenant } from 'src/app/modules/saas-manage/model/tenant';

@Injectable({
  providedIn: 'root'
})
export class TenantService {

  constructor(private httpClient: HttpClient) { }

  getTenant(id) {
    return this.httpClient.get(`${UriConstant.TenantUri}?id=${id}`);
  }

  getTenants(page, size, name, dataBaseType, sortKey, isDesc) {
    var uri = `${UriConstant.TenantUri}/list?page=${page}&&size=${size}`;
    uri += name ? `&&name=${name}` : '';
    uri += dataBaseType ? `&&dataBaseType=${dataBaseType}` : '';
    uri += sortKey ? `&&sortKey=${sortKey}` : '';
    uri += isDesc ? `&&isDesc=${isDesc}` : '';
    return this.httpClient.get(uri);
  }

  delete(id: String) {
    return this.httpClient.delete(`${UriConstant.TenantUri}?id=${id}`);
  }

  update(tenant: Tenant) {
    return this.httpClient.patch(UriConstant.TenantUri, tenant);
  }

  add(tenant: Tenant) {
    return this.httpClient.post(UriConstant.TenantUri, tenant);
  }
}
