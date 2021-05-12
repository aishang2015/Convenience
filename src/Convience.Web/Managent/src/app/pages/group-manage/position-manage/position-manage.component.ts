import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { PositionService } from 'src/app/business/group-manage/position.service';
import { Position } from '../model/position';

@Component({
  selector: 'app-position-manage',
  templateUrl: './position-manage.component.html',
  styleUrls: ['./position-manage.component.less']
})
export class PositionManageComponent implements OnInit {

  isSpinning: boolean = false;

  data: Position[] = [];

  size: number = 10;
  page: number = 1;
  total: number = 0;

  currentId?: number = null;

  @ViewChild('editTitleTpl', { static: true })
  editTitleTpl;

  @ViewChild('addTitleTpl', { static: true })
  addTitleTpl;

  @ViewChild('contentTpl', { static: true })
  contentTpl;

  editForm: FormGroup = new FormGroup({});

  modal: NzModalRef;

  constructor(
    private _positionService: PositionService,
    private _messageService: NzMessageService,
    private _modalService: NzModalService,
    private _formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.refresh();
  }

  add() {
    this.editForm = this._formBuilder.group({
      name: [null, [Validators.required, Validators.maxLength(10)]],
      sort: [null, [Validators.required]],
    });
    this.currentId = null;
    this.modal = this._modalService.create({
      nzTitle: this.addTitleTpl,
      nzContent: this.contentTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });

  }

  refresh() {
    this.isSpinning = true;
    this._positionService.get(this.page, this.size).subscribe((result: any) => {
      this.data = result['data'];
      this.total = result['count'];
      this.isSpinning = false;
    }, () => this.isSpinning = false);
  }

  edit(id) {
    this._positionService.getPosition(id).subscribe(result => {
      this.currentId = id;
      this.editForm = this._formBuilder.group({
        name: [result['name'], [Validators.required, Validators.maxLength(10)]],
        sort: [result['sort'], [Validators.required]],
      });
      this.modal = this._modalService.create({
        nzTitle: this.editTitleTpl,
        nzContent: this.contentTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });
  }

  remove(id) {
    this._modalService.confirm({
      nzTitle: '是否删除该职位?',
      nzContent: null,
      nzOnOk: () => this._positionService.delete(id).subscribe(() => {
        this._messageService.success("删除成功！");
        this.refresh();
      }),
    });

  }

  submitEdit() {

    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      let p = new Position();
      p.name = this.editForm.value['name'];
      p.sort = this.editForm.value['sort'];
      if (this.currentId == null) {
        this._positionService.add(p).subscribe(() => {
          this._messageService.success("添加成功！");
          this.refresh();
          this.modal.close();
        });
      } else {
        p.id = this.currentId;
        this._positionService.update(p).subscribe(() => {
          this._messageService.success("修改成功！");
          this.refresh();
          this.modal.close();
        });
      }
    }
  }

  cancel() {
    this.modal.close();
  }

  pageChange() {
    this.refresh();
  }

  sizeChange() {
    this.page = 1;
    this.refresh();
  }

}
