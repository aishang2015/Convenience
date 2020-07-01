import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NzMessageService, NzModalService, NzModalRef, NzTreeNodeOptions } from 'ng-zorro-antd';
import { Article } from '../model/article';
import { Column } from '../model/column';
import { ColumnService } from 'src/app/business/column.service';
import { ArticleService } from 'src/app/business/article.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-article-manage',
  templateUrl: './article-manage.component.html',
  styleUrls: ['./article-manage.component.scss']
})
export class ArticleManageComponent implements OnInit {

  data: Article[] = [];

  nodes: NzTreeNodeOptions[] = [];

  size: number = 10;
  page: number = 1;
  total: number = 0;

  @ViewChild('contentTpl', { static: true })
  contentTpl;

  searchForm: FormGroup = new FormGroup({});

  modal: NzModalRef;

  constructor(
    private _messageService: NzMessageService,
    private _modalService: NzModalService,
    private _formBuilder: FormBuilder,
    private _columnService: ColumnService,
    private _articleService: ArticleService,
    private _router: Router) { }

  ngOnInit(): void {
    this.searchForm = this._formBuilder.group({
      title: [],
      tag: [],
      columnId: []
    });
    this.initNodes();
    this.refresh();
  }

  initNodes() {
    let nodes: NzTreeNodeOptions[] = [];
    this._columnService.getAll().subscribe((result: any) => {
      this.makeNodes(null, nodes, result);
      this.nodes = nodes;
    });
  }

  makeNodes(upId, nodes, columns: Column[]) {
    var cs = columns.filter(column => column.upId == upId);
    cs.forEach(column => {
      let data = { title: column.name, key: column.id, icon: 'database', children: [] };
      this.makeNodes(column.id, data.children, columns);
      nodes.push(data);
    });
  }

  add() {
    this._router.navigate(['/content/article/edit', { id: '' }]);
  }

  edit(id) {
    this._router.navigate(['/content/article/edit', { id: id }]);
  }

  refresh() {
    this._articleService.getList(this.page, this.size,
      this.searchForm.value['title'], this.searchForm.value['tag'],
      this.searchForm.value['columnId'])
      .subscribe(result => {
        this.data = result['data'];
        this.total = result['count'];
      });
  }

  remove(id) {
    this._modalService.confirm({
      nzTitle: '是否删除该文章?',
      nzContent: null,
      nzOnOk: () => {
        this._articleService.delete(id).subscribe(result => {
          this._messageService.success("删除成功！");
          this.refresh();
        })
      },
    });
  }

  submitSearch() {
    this.refresh();
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

  getTagList(tags: string) {
    return tags?.split(',').filter(e => e);
  }

}
