import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpResponse } from '@angular/common/http';
import { UriConfig } from '../configs/uri-config';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FileService {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConfig) { }


  upload(directory, fileList) {
    const formData = new FormData();
    formData.append('currentDirectory', directory);
    fileList.forEach((file: any) => {
      formData.append('files', file);
    });
    return this.httpClient.post(this.uriConstant.FileUri, formData);
  }

  get(page, size, directory) {
    let uri = `${this.uriConstant.FileUri}/list?page=${page}&&size=${size}&&directory=${directory}`;
    return this.httpClient.get(uri);
  }

  delete(fileName, directory) {
    let uri = `${this.uriConstant.FileUri}?fileName=${fileName}&&directory=${directory}`;
    return this.httpClient.delete(uri);
  }

  download(fileName, directory) {
    let uri = `${this.uriConstant.FileUri}?fileName=${fileName}&&directory=${directory}`;
    return this.httpClient.get(uri, { responseType: "blob", });
  }
}
