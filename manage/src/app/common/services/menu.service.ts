import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../constants/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  constructor(private httpClient: HttpClient) { }

  get() {
    return this.httpClient.get(UriConstant.MenuUri);
  }

  delete(id) {
    return this.httpClient.delete(`${UriConstant.MenuUri}?id=${id}`);
  }

  update(menu) {
    return this.httpClient.patch(UriConstant.MenuUri, menu);
  }

  add(menu) {
    return this.httpClient.post(UriConstant.MenuUri, menu);
  }
}
