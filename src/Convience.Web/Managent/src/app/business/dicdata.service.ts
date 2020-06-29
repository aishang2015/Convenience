
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from '../configs/uri-config';

@Injectable({
  providedIn: 'root'
})
export class DicDataService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }

  get(id) {
    return this.httpClient.get(`${this.uriConstant.DicDataUri}?id=${id}`);
  }

  getList(dictypeId) {
    let uri = `${this.uriConstant.DicDataUri}/list?dicTypeId=${dictypeId}`;
    return this.httpClient.get(uri);
  }

  delete(id) {
    return this.httpClient.delete(`${this.uriConstant.DicDataUri}?id=${id}`);
  }

  update(dicdata) {
    return this.httpClient.patch(this.uriConstant.DicDataUri, dicdata);
  }

  add(dicdata) {
    return this.httpClient.post(this.uriConstant.DicDataUri, dicdata);
  }
}
