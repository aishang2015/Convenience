import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NzMessageService, NzModalService, NzModalRef, NzTreeNodeOptions } from 'ng-zorro-antd';
import { Article } from '../model/article';
import { Column } from '../model/column';
import { ColumnService } from 'src/app/services/column.service';
import { ArticleService } from 'src/app/services/article.service';
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
    private messageService: NzMessageService,
    private modalService: NzModalService,
    private formBuilder: FormBuilder,
    private columnService: ColumnService,
    private articleService: ArticleService,
    private router: Router) { }

  ngOnInit(): void {
    this.searchForm = this.formBuilder.group({
      title: [],
      tag: [],
      columnId: []
    });
    this.initNodes();
    this.refresh();
  }

  initNodes() {
    let nodes: NzTreeNodeOptions[] = [];
    this.columnService.getAll().subscribe((result: any) => {
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
    this.router.navigate(['/content/article/edit', { id: '' }]);
  }

  edit(id) {
    this.router.navigate(['/content/article/edit', { id: id }]);
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

  remove(id) {
    this.modalService.confirm({
      nzTitle: '是否删除该文章?',
      nzContent: null,
      nzOnOk: () => {
        this.articleService.delete(id).subscribe(result => {
          this.messageService.success("删除成功！");
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
