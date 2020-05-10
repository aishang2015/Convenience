
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/constants/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class DicTypeService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  get(id) {
    return this.httpClient.get(`${this.uriConstant.DicTypeUri}?id=${id}`);
  }

  getList() {
    let uri = `${this.uriConstant.DicTypeUri}/list`;
    return this.httpClient.get(uri);
  }

  delete(id) {
    return this.httpClient.delete(`${this.uriConstant.DicTypeUri}?id=${id}`);
  }

  update(dictype) {
    return this.httpClient.patch(this.uriConstant.DicTypeUri, dictype);
  }

  add(dictype) {
    return this.httpClient.post(this.uriConstant.DicTypeUri, dictype);
  }
}
