import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from '../configs/uri-config';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }

  get(id) {
    return this.httpClient.get(`${this.uriConstant.ArticleUri}?id=${id}`);
  }

  getList(page, size, title, tag, columnId) {
    let uri = `${this.uriConstant.ArticleUri}/list?page=${page}&&size=${size}`;
    uri += title ? `&&title=${title}` : '';
    uri += tag ? `&&tag=${tag}` : '';
    uri += columnId ? `&&columnId=${columnId}` : '';
    return this.httpClient.get(uri);
  }

  delete(id) {
    return this.httpClient.delete(`${this.uriConstant.ArticleUri}?id=${id}`);
  }

  update(article) {
    return this.httpClient.patch(this.uriConstant.ArticleUri, article);
  }

  add(article) {
    return this.httpClient.post(this.uriConstant.ArticleUri, article);
  }
}
