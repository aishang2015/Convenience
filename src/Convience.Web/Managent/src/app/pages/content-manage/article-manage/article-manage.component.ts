import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Article } from '../model/article';
import { Column } from '../model/column';
import { ColumnService } from 'src/app/business/column.service';
import { ArticleService } from 'src/app/business/article.service';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { NzFormatEmitEvent, NzTreeNode, NzTreeNodeOptions } from 'ng-zorro-antd/tree';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-article-manage',
  templateUrl: './article-manage.component.html',
  styleUrls: ['./article-manage.component.less']
})
export class ArticleManageComponent implements OnInit {

  data: Article[] = [];
  treeData: any[] = [];

  nodes: NzTreeNodeOptions[] = [];

  size: number = 10;
  page: number = 1;
  total: number = 0;


  currentId?: number = null;

  editForm: FormGroup = new FormGroup({});

  @ViewChild('contentTpl', { static: true })
  contentTpl;

  @ViewChild('columnTpl', { static: true })
  columnTpl;

  @ViewChild('preViewTpl', { static: true })
  preViewTpl;

  searchForm: FormGroup = new FormGroup({});

  modal: NzModalRef;

  selectedNode: NzTreeNode;

  searchTitle: string;

  searchTag: string;

  viewedArticle: Article;

  constructor(
    private _messageService: NzMessageService,
    private _modalService: NzModalService,
    private _formBuilder: FormBuilder,
    private _columnService: ColumnService,
    private _articleService: ArticleService,
    private _sanitized: DomSanitizer,
    private _router: Router) { }

  ngOnInit(): void {
    this.searchForm = this._formBuilder.group({
      title: [],
      tag: []
    });
    this.initNodes();
    this.refresh();
  }

  initNodes() {
    let nodes: NzTreeNodeOptions[] = [{ title: '文章栏目', key: null, icon: 'database', expanded: true, children: [] }];
    this._columnService.getAll().subscribe((result: any) => {
      this.treeData = result;
      this.makeNodes(null, nodes[0], this.treeData);
      this.nodes = nodes;
    });
  }

  makeNodes(upId, node, columns: Column[]) {
    var cs = columns.filter(column => column.upId == upId);
    cs.forEach(column => {
      let data = { title: column.name, key: column.id, icon: 'database', children: [] };
      this.makeNodes(column.id, data, columns);
      node.children.push(data);
    });
  }

  add() {
    this._router.navigate(['/content/article/edit', { id: '' }]);
  }

  edit(id) {
    this._router.navigate(['/content/article/edit', { id: id }]);
  }

  viewArticle(id) {
    this._articleService.get(id).subscribe((result: any) => {
      this.viewedArticle = result;
      this._modalService.create({
        nzTitle: "文章预览",
        nzContent: this.preViewTpl,
        nzFooter: null,
        nzWidth: "70%"
      });
    });
  }

  pass(content) {
    return this._sanitized.bypassSecurityTrustHtml(content);
  }

  refresh() {
    this._articleService.getList(
      this.page,
      this.size,
      this.searchTitle,
      this.searchTag,
      this.selectedNode?.key)
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
    this.searchTitle = this.searchForm.value["title"];
    this.searchTag = this.searchForm.value["tag"];
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


  treeClick(event: NzFormatEmitEvent) {
    this.selectedNode = event.keys.length > 0 ? event.node : null;
    this.refresh();
  }

  addColumn() {
    this.currentId = null;
    this.editForm = this._formBuilder.group({
      upColumn: [this.selectedNode?.key],
      name: [null, [Validators.required, Validators.maxLength(15)]],
      sort: [null, [Validators.required]]
    });
    this.modal = this._modalService.create({
      nzTitle: '添加栏目',
      nzContent: this.columnTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  editColumn() {
    this._columnService.get(this.selectedNode?.key).subscribe((result: Column) => {
      this.currentId = result.id;
      let upId = this.treeData.find(d => d.id == result.id)?.upId;
      this.editForm = this._formBuilder.group({
        upColumn: [{ value: Number(upId), disabled: true }],
        name: [result.name, [Validators.required, Validators.maxLength(15)]],
        sort: [result.sort, Validators.required]
      });

      this.modal = this._modalService.create({
        nzTitle: '编辑栏目',
        nzContent: this.columnTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });
  }

  deleteColumn() {
    this._modalService.confirm({
      nzTitle: '是否删除该栏目?',
      nzContent: null,
      nzOnOk: () => {
        this._columnService.delete(this.selectedNode?.key).subscribe(() => {
          this.initNodes();
          this._messageService.success("删除成功！");
        });
      },
    });
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      let column = new Column();
      column.name = this.editForm.value["name"];
      column.sort = this.editForm.value["sort"];
      column.upId = this.editForm.value["upColumn"];

      if (this.currentId) {
        column.id = this.currentId;
        this._columnService.update(column).subscribe(() => {
          this._messageService.success("修改成功！");
          this.initNodes();
          this.modal.close();
        });
      } else {
        this._columnService.add(column).subscribe(() => {
          this._messageService.success("添加成功！");
          this.initNodes();
          this.modal.close();
        });
      }
    }
  }

}
