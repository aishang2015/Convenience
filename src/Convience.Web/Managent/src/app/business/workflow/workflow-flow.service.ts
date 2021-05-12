import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from '../../configs/uri-config';

@Injectable({
  providedIn: 'root'
})
export class WorkflowFlowService {


  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }

  get(id) {
    return this.httpClient.get(`${this.uriConstant.WorkFlowFlowUri}?workflowId=${id}`);
  }

  addOrUpdate(data) {
    return this.httpClient.post(this.uriConstant.WorkFlowFlowUri, data);
  }
}
