import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/constants/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class ColumnService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  getAll() {
    return this.httpClient.get(`${this.uriConstant.ColumnUri}/all`);
  }

  get(id) {
    return this.httpClient.get(`${this.uriConstant.ColumnUri}?id=${id}`);
  }

  delete(id) {
    return this.httpClient.delete(`${this.uriConstant.ColumnUri}?id=${id}`);
  }

  update(column) {
    return this.httpClient.patch(this.uriConstant.ColumnUri, column);
  }

  add(column) {
    return this.httpClient.post(this.uriConstant.ColumnUri, column);
  }
}
