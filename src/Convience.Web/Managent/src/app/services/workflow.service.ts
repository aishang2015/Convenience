import { Injectable } from '@angular/core';
import { UriConstant } from '../core/constants/uri-constant';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class WorkflowService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }


  getList(page, size, groupId) {
    let uri = `${this.uriConstant.WorkFlowUri}/list?page=${page}&&size=${size}&&workFlowGroupId=${groupId}`;
    return this.httpClient.get(uri);
  }

  get(id) {
    return this.httpClient.get(`${this.uriConstant.WorkFlowUri}?id=${id}`);
  }

  delete(id) {
    return this.httpClient.delete(`${this.uriConstant.WorkFlowUri}?id=${id}`);
  }

  update(workflow) {
    return this.httpClient.patch(this.uriConstant.WorkFlowUri, workflow);
  }

  add(workflow) {
    return this.httpClient.post(this.uriConstant.WorkFlowUri, workflow);
  }
}
