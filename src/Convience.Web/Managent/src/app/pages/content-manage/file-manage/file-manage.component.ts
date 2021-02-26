import { Component, OnInit, ViewChild } from '@angular/core';
import { FileInfo } from '../model/fileInfo';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzMessageService } from 'ng-zorro-antd/message';
import { FileService } from 'src/app/business/file.service';
import { FolderService } from 'src/app/business/folder.service';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-file-manage',
  templateUrl: './file-manage.component.html',
  styleUrls: ['./file-manage.component.less']
})
export class FileManageComponent implements OnInit {

  folderForm: FormGroup = new FormGroup({});

  fileInfoList: FileInfo[] = [];

  folderList: string[] = [];

  @ViewChild('uploadTitleTpl', { static: true })
  uploadTitleTpl;

  @ViewChild('uploadContentTpl', { static: true })
  uploadContentTpl;

  @ViewChild('uploadFooterTpl', { static: true })
  uploadFooterTpl;

  @ViewChild('folderContentTpl', { static: true })
  folderContentTpl;

  modal: NzModalRef;

  fileList: any[] = [];

  uploading: boolean = false;

  currentDirectory: string = '/';

  emptyValidator = (control: FormControl): { [key: string]: any } | null => {
    const reg = /^\s+/g;
    let value = control.value?.replace(reg, '');
    return value ? null : { 'notEmpty': true };
  };

  constructor(
    private _modalService: NzModalService,
    private _messageService: NzMessageService,
    private _fileService: FileService,
    private _folderService: FolderService,
    private _formBuilder: FormBuilder) {
  }

  ngOnInit(): void {
    this.refresh();
  }

  dbc(file: FileInfo) {
    if (file.isDirectory) {
      let dir = file.directory + '/' + file.fileName
      this.folderList = dir.split('/').filter(e => e);
      this.currentDirectory = '/' + this.folderList.join('/');
      this.refresh();
    }
  }

  refresh() {
    this._fileService.get(1, 200, this.currentDirectory).subscribe((result: any) => {
      this.fileInfoList = result;
    });
  }

  upload() {
    this.modal = this._modalService.create({
      nzTitle: this.uploadTitleTpl,
      nzContent: this.uploadContentTpl,
      nzFooter: this.uploadFooterTpl,
      nzMaskClosable: false,
    })
  }

  beforeUpload = (file): boolean => {
    this.fileList = this.fileList.concat(file);
    return false;
  };

  handleUpload() {
    this.uploading = true;
    this._fileService.upload(this.currentDirectory, this.fileList).subscribe(result => {
      this.modal.close();
      this._messageService.success("上传完毕！");
      this.refresh();
      this.fileList = [];
      this.uploading = false;
    }, error => {
      this.uploading = false;
    });
  }

  delete(fileInfo: FileInfo) {
    this._modalService.confirm({
      nzTitle: '是否删除?',
      nzContent: null,
      nzOnOk: () => {
        if (fileInfo.isDirectory) {
          this._folderService.deleteFolder(fileInfo.directory, fileInfo.fileName).subscribe(result => {
            this._messageService.success("删除成功！");
            this.refresh();
          });
        } else {
          this._fileService.delete(fileInfo.fileName, fileInfo.directory).subscribe(result => {
            this._messageService.success("删除成功！");
            this.refresh();
          });
        }
      }
    });
  }

  download(fileInfo: FileInfo) {
    this._fileService.download(fileInfo.fileName, fileInfo.directory).subscribe((result: any) => {
      const a = document.createElement('a');
      const blob = new Blob([result], { 'type': "application/octet-stream" });
      a.href = URL.createObjectURL(blob);
      a.download = fileInfo.fileName;
      a.click();
    });
  }

  createFolder() {
    this.modal = this._modalService.create({
      nzTitle: '创建文件夹',
      nzContent: this.folderContentTpl,
      nzFooter: null
    });
    this.folderForm = this._formBuilder.group({
      folderName: [null, [Validators.required, Validators.maxLength(15),
      Validators.pattern('^[^\\\\/:\*\?""<>|]{1,120}$'), this.emptyValidator]]
    });
  }

  submit() {
    for (const i in this.folderForm.controls) {
      this.folderForm.controls[i].markAsDirty();
      this.folderForm.controls[i].updateValueAndValidity();
    }
    if (this.folderForm.valid) {
      this._folderService.addFolder(this.currentDirectory, this.folderForm.value['folderName']).subscribe(result => {
        this._messageService.success("文件夹创建成功！");
        this.refresh();
        this.modal.close();
      });
    }
  }

  cancel() {
    this.modal.close();
  }

  navgiateFolder(index) {
    if (index == -1) {
      this.currentDirectory = '/';
      this.folderList = [];
    } else {
      this.folderList = this.folderList.slice(0, index + 1);
      this.currentDirectory = '/' + this.folderList.join('/');
    }
    this.refresh();
  }

  back() {
    if (this.folderList.length >= 1) {
      this.folderList = this.folderList.slice(0, this.folderList.length - 1);
      this.currentDirectory = '/' + this.folderList.join('/');
      this.refresh();
    }
  }
}
