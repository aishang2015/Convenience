import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/constants/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class WorkflowGroupService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  getAll() {
    return this.httpClient.get(`${this.uriConstant.WorkFlowGroupUri}/all`);
  }

  get(id) {
    return this.httpClient.get(`${this.uriConstant.WorkFlowGroupUri}?id=${id}`);
  }

  delete(id) {
    return this.httpClient.delete(`${this.uriConstant.WorkFlowGroupUri}?id=${id}`);
  }

  update(workflowGroup) {
    return this.httpClient.patch(this.uriConstant.WorkFlowGroupUri, workflowGroup);
  }

  add(workflowGroup) {
    return this.httpClient.post(this.uriConstant.WorkFlowGroupUri, workflowGroup);
  }

}
