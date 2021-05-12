import { Injectable } from '@angular/core';
import { UriConfig } from '../../configs/uri-config';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class WorkflowService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }


  getList(page, size, groupId, ispublish?) {
    let uri = `${this.uriConstant.WorkFlowUri}/list?page=${page}&&size=${size}&&workFlowGroupId=${groupId}`;
    if (ispublish) uri = uri + `&&isPublish=${ispublish}`;
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

  publish(id, value) {
    return this.httpClient.put(this.uriConstant.WorkFlowUri, {
      id: id,
      isPublish: value,
    });
  }
}
