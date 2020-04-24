import { Component, OnInit, ViewChild } from '@angular/core';
import { NzIconService } from 'ng-zorro-antd/icon';
import { FileInfo } from '../model/fileInfo';
import { NzModalService, NzModalRef, UploadFile, NzMessageService } from 'ng-zorro-antd';
import { FileService } from 'src/app/services/file.service';
import { FolderService } from 'src/app/services/folder.service';

@Component({
  selector: 'app-file-manage',
  templateUrl: './file-manage.component.html',
  styleUrls: ['./file-manage.component.scss']
})
export class FileManageComponent implements OnInit {

  fileInfoList: FileInfo[] = [];

  folderList: string[] = [];

  @ViewChild('uploadTitleTpl', { static: true })
  uploadTitleTpl;

  @ViewChild('uploadContentTpl', { static: true })
  uploadContentTpl;

  @ViewChild('uploadFooterTpl', { static: true })
  uploadFooterTpl;

  modal: NzModalRef;

  fileList: UploadFile[] = [];

  uploading: boolean = false;

  currentDirectory: string = '';

  constructor(private modalService: NzModalService,
    private messageService: NzMessageService,
    private fileService: FileService,
    private folderService: FolderService) {
  }

  ngOnInit(): void {
    this.refresh();
  }

  dbc(dir) {
    this.currentDirectory = dir;
    this.folderList = dir.split('/').filter(e => e);
  }

  refresh() {
    this.fileService.get(1, 200, this.currentDirectory).subscribe((result: any) => {
      this.fileInfoList = result;
    });
  }

  upload() {
    this.modal = this.modalService.create({
      nzTitle: this.uploadTitleTpl,
      nzContent: this.uploadContentTpl,
      nzFooter: this.uploadFooterTpl,
      nzMaskClosable: false,
    })
  }

  beforeUpload = (file: UploadFile): boolean => {
    this.fileList = this.fileList.concat(file);
    return false;
  };

  handleUpload() {
    this.uploading = true;
    this.fileService.upload(this.currentDirectory, this.fileList).subscribe(result => {
      this.modal.close();
      this.messageService.success("上传完毕！");
      this.refresh();
      this.fileList = [];
      this.uploading = false;
    }, error => {
      this.uploading = false;
    });
  }

  delete(fileInfo: FileInfo) {
    this.fileService.delete(fileInfo.fileName, fileInfo.directory).subscribe(result => {
      this.messageService.success("删除成功！");
      this.refresh();
    });
  }

  download(fileInfo: FileInfo) {
    this.fileService.download(fileInfo.fileName, fileInfo.directory).subscribe((result: any) => {
      const a = document.createElement('a');
      const blob = new Blob([result], { 'type': "application/octet-stream" });
      a.href = URL.createObjectURL(blob);
      a.download = fileInfo.fileName;
      a.click();
    });
  }
}
