import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/constants/uri-constant';

@Injectable({
  providedIn: 'root',
})
export class FolderService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }


  addFolder(directory: string, folderName: string) {
    return this.httpClient.post(this.uriConstant.FolderUri, { directory: directory, fileName: folderName });
  }

  deleteFolder(directory: string, folderName: string) {
    return this.httpClient.delete(`${this.uriConstant.FolderUri}?directory=${directory}&&fileName=${folderName}`)
  }

}
