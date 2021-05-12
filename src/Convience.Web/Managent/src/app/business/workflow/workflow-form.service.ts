import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from '../../configs/uri-config';

@Injectable({
  providedIn: 'root'
})
export class WorkflowFormService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }

  get(id) {
    return this.httpClient.get(`${this.uriConstant.WorkFlowFormUri}?workflowId=${id}`);
  }

  addOrUpdate(data) {
    return this.httpClient.post(this.uriConstant.WorkFlowFormUri, data);
  }

  getControlDic(id) {
    return this.httpClient.get(`${this.uriConstant.WorkFlowFormUri}/dic?workflowId=${id}`);
  }
}
