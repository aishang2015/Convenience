import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NzIconService } from 'ng-zorro-antd/icon';
import { FileInfo } from '../model/fileInfo';

@Component({
  selector: 'app-file-icon',
  templateUrl: './file-icon.component.html',
  styleUrls: ['./file-icon.component.less']
})
export class FileIconComponent implements OnInit {

  @Input()
  file: FileInfo = new FileInfo();

  @Output()
  dbClick = new EventEmitter<FileInfo>();

  @Output()
  deleteClick = new EventEmitter<FileInfo>();

  @Output()
  downloadClick = new EventEmitter<FileInfo>();

  private isFirstClick = true;
  private fristClickTime = Date.now();

  constructor(private _iconService: NzIconService) { }

  ngOnInit(): void {
    this._iconService.fetchFromIconfont({
      scriptUrl: 'https://at.alicdn.com/t/font_1777474_z6ft00wvau.js'
    });
  }

  click() {
    let clickSpan = 300;
    if (this.isFirstClick) {
      this.isFirstClick = false;
      this.fristClickTime = Date.now();
      setTimeout(() => {
        this.isFirstClick = true;
      }, clickSpan);
    } else {
      this.dbClick.emit(this.file);
      this.isFirstClick = true;
    }
  }

  download() {
    this.downloadClick.emit(this.file);
  }
  delete() {
    this.deleteClick.emit(this.file);
  }

  getIconType() {
    if (this.file.isDirectory) {
      return 'iconyunpanlogo-16';
    }
    var endfix = this.file.fileName.split('.').reverse()[0];
    if (['png', 'jpg', 'jpeg', 'bmp', 'gif', 'webp', 'psd', 'svg', 'tiff'].includes(endfix)) {
      return 'iconyunpanlogo-18';
    } else if (['pdf'].includes(endfix)) {
      return 'iconyunpanlogo-19';
    } else if (['xls', 'xlsx'].includes(endfix)) {
      return 'iconyunpanlogo-1';
    } else if (['doc', 'docx'].includes(endfix)) {
      return 'iconyunpanlogo-3';
    } else if (['ppt', 'pptx'].includes(endfix)) {
      return 'iconyunpanlogo-2';
    } else if (['mp4', 'rmvb', 'avi'].includes(endfix)) {
      return 'iconyunpanlogo-10';
    } else {
      return 'iconyunpanlogo-6';
    }

  }

}
