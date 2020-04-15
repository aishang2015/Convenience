import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/constants/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  get() {
    return this.httpClient.get(this.uriConstant.DashboardUri);
  }




}
