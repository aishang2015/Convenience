import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/constants/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class PositionService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  get(page, size) {
    var uri = `${this.uriConstant.PositiondUri}/list?page=${page}&&size=${size}`;
    return this.httpClient.get(uri);
  }

  getAll() {
    var uri = `${this.uriConstant.UserUri}/all`;
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
}
