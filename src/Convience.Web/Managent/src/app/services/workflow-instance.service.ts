import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/constants/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class WorkflowInstanceService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  createInstance(id) {
    return this.httpClient.post(this.uriConstant.WorkFlowInstanceUri, { WorkFlowId: id });
  }

  getInstances(page, size) {
    return this.httpClient.get(`${this.uriConstant.WorkFlowInstanceUri}?page=${page}&&size=${size}`);
  }

  getControlValue(id) {
    return this.httpClient.get(`${this.uriConstant.WorkFlowInstanceUri}/values?workFlowInstanceId=${id}`);
  }

  saveControlValues(data) {
    return this.httpClient.put(`${this.uriConstant.WorkFlowInstanceUri}/values`, data);
  }
}
