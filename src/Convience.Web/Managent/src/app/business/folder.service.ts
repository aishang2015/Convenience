import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConfig } from '../configs/uri-config';

@Injectable({
  providedIn: 'root',
})
export class FolderService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }


  addFolder(directory: string, folderName: string) {
    return this.httpClient.post(this.uriConstant.FolderUri, { directory: directory, fileName: folderName });
  }

  deleteFolder(directory: string, folderName: string) {
    return this.httpClient.delete(`${this.uriConstant.FolderUri}?directory=${directory}&&fileName=${folderName}`)
  }

}
