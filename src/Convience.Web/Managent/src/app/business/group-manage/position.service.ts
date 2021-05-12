import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from '../../configs/uri-config';

@Injectable({
  providedIn: 'root'
})
export class PositionService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }

  get(page, size) {
    var uri = `${this.uriConstant.PositiondUri}/list?page=${page}&&size=${size}`;
    return this.httpClient.get(uri);
  }

  getAll() {
    var uri = `${this.uriConstant.PositiondUri}/all`;
    return this.httpClient.get(uri);
  }

  getPosition(id) {
    return this.httpClient.get(`${this.uriConstant.PositiondUri}?id=${id}`);
  }

  delete(id) {
    return this.httpClient.delete(`${this.uriConstant.PositiondUri}?id=${id}`);
  }

  update(position) {
    return this.httpClient.patch(this.uriConstant.PositiondUri, position);
  }

  add(position) {
    return this.httpClient.post(this.uriConstant.PositiondUri, position);
  }

  getDic(name) {
    let uri = `${this.uriConstant.PositiondUri}/dic?name=${name}`;
    return this.httpClient.get(uri);
  }
}
