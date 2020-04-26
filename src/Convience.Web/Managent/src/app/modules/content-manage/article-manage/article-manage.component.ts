import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NzMessageService, NzModalService, NzModalRef } from 'ng-zorro-antd';
import { Article } from '../model/article';
import { Column } from '../model/column';
import { ColumnService } from 'src/app/services/column.service';
import { ArticleService } from 'src/app/services/article.service';

@Component({
  selector: 'app-article-manage',
  templateUrl: './article-manage.component.html',
  styleUrls: ['./article-manage.component.scss']
})
export class ArticleManageComponent implements OnInit {

  data: Article[] = [];

  columns: Column[] = [];

  size: number = 10;
  page: number = 1;
  total: number = 0;

  currentId?: number = null;

  @ViewChild('contentTpl', { static: true })
  contentTpl;

  editForm: FormGroup = new FormGroup({});

  searchForm: FormGroup = new FormGroup({});

  modal: NzModalRef;

  constructor(
    private messageService: NzMessageService,
    private modalService: NzModalService,
    private formBuilder: FormBuilder,
    private columnService: ColumnService,
    private articleService: ArticleService) { }

  ngOnInit(): void {
    this.searchForm = this.formBuilder.group({
      title: [],
      tag: [],
      columnId: []
    });
    this.refresh();
  }

  initColumns() {
    this.columnService.getAll().subscribe((result: Column[]) => this.columns = result);
  }

  add() {
    this.currentId = null;
    this.modal = this.modalService.create({
      nzTitle: '',
      nzContent: this.contentTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });

  }

  refresh() {
    this.articleService.getList(this.page, this.size,
      this.searchForm.value['title'], this.searchForm.value['tag'],
      this.searchForm.value['columnId'])
      .subscribe(result => {
        this.data = result['data'];
        this.total = result['count'];
      });
  }

  edit(id) {
    this.currentId = id;

  }

  remove(id) {
    this.modalService.confirm({
      nzTitle: '是否删除该?',
      nzContent: null,
      nzOnOk: () => { },
    });
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      if (this.currentId) {

      } else {

      }
    }
  }

  submitSearch() {
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
