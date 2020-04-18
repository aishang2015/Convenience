import { Component, OnInit, ElementRef, ViewChild, TemplateRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NzMessageService, NzModalService, NzModalRef } from 'ng-zorro-antd';
import { PositionService } from 'src/app/services/position.service';
import { Position } from '../model/position';

@Component({
  selector: 'app-position-manage',
  templateUrl: './position-manage.component.html',
  styleUrls: ['./position-manage.component.scss']
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
    private positionService: PositionService,
    private messageService: NzMessageService,
    private modalService: NzModalService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.refresh();
  }

  add() {
    this.editForm = this.formBuilder.group({
      name: [null, [Validators.required, Validators.maxLength(10)]],
      sort: [null, [Validators.required]],
    });
    this.currentId = null;
    this.modal = this.modalService.create({
      nzTitle: this.editTitleTpl,
      nzContent: this.contentTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });

  }

  refresh() {
    this.isSpinning = true;
    this.positionService.get(this.page, this.size).subscribe((result: any) => {
      this.data = result['data'];
      this.total = result['count'];
      this.isSpinning = false;
    }, error => this.isSpinning = false);
  }

  edit(id) {
    this.positionService.getPosition(id).subscribe(result => {
      this.currentId = id;
      this.editForm = this.formBuilder.group({
        name: [result['name'], [Validators.required, Validators.maxLength(10)]],
        sort: [result['sort'], [Validators.required]],
      });
      this.modal = this.modalService.create({
        nzTitle: this.editTitleTpl,
        nzContent: this.contentTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });
  }

  remove(id) {
    this.modalService.confirm({
      nzTitle: '是否删除该职位?',
      nzContent: null,
      nzOnOk: () => this.positionService.delete(id).subscribe(result => {
        this.messageService.success("删除成功！");
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
        this.positionService.add(p).subscribe(result => {
          this.messageService.success("添加成功！");
          this.refresh();
          this.modal.close();
        });
      } else {
        p.id = this.currentId;
        this.positionService.update(p).subscribe(result => {
          this.messageService.success("修改成功！");
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
