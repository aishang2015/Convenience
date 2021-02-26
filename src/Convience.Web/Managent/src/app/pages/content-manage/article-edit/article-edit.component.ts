import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ArticleService } from 'src/app/business/article.service';
import { ColumnService } from 'src/app/business/column.service';
import { Column } from '../model/column';
import { Article } from '../model/article';
import { StorageService } from 'src/app/services/storage.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzTreeNodeOptions } from 'ng-zorro-antd/tree';

@Component({
  selector: 'app-article-edit',
  templateUrl: './article-edit.component.html',
  styleUrls: ['./article-edit.component.less']
})
export class ArticleEditComponent implements OnInit {

  currentId?: number = null;

  columns: Column[] = [];

  editForm: FormGroup = new FormGroup({});

  nodes: NzTreeNodeOptions[] = [];

  isLoading: boolean = false;

  tinyConfig = {
    height: 800,
    language: 'zh_CN',
    language_url: '../../assets/tinymce/langs/zh_CN.js',
    plugins: 'print preview paste importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons',
    imagetools_cors_hosts: ['picsum.photos'],
    menubar: 'file edit view insert format tools table help',
    toolbar_mode: 'wrap',
    toolbar: 'undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample | ltr rtl',

  };

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _articleService: ArticleService,
    private _messageService: NzMessageService,
    private _columnService: ColumnService,
    private _formBuilder: FormBuilder,
    private _storageService: StorageService) { }

  ngOnInit(): void {
    this._storageService.clearTinymceCache();
    this.editForm = this._formBuilder.group({
      title: [null, [Validators.required, Validators.maxLength(50)]],
      subTitle: [null, [Validators.maxLength(200)]],
      columnId: [null],
      source: [null, [Validators.maxLength(200)]],
      sort: [null, [Validators.required]],
      tags: [null, [Validators.maxLength(200)]],
      content: [null]
    });
    let id = this._activatedRoute.snapshot.paramMap.get('id')?.trim();
    this.currentId = id ? Number(id) : null;
    if (this.currentId) {
      this._articleService.get(this.currentId).subscribe((result: any) => {
        this.editForm = this._formBuilder.group({
          title: [result['title'], [Validators.required, Validators.maxLength(50)]],
          subTitle: [result['subTitle'], [Validators.maxLength(200)]],
          columnId: [result['columnId']],
          source: [result['source'], [Validators.maxLength(200)]],
          sort: [result['sort'], [Validators.required]],
          tags: [result['tags'], [Validators.maxLength(200)]],
          content: [result['content']]
        });
      });
    }
    this.initNodes();
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      this.isLoading = true;
      let article = new Article();
      article.title = this.editForm.value['title'];
      article.subTitle = this.editForm.value['subTitle'];
      article.columnId = this.editForm.value['columnId'];
      article.columnName = this.editForm.value['columnName'];
      article.source = this.editForm.value['source'];
      article.sort = this.editForm.value['sort'];
      article.tags = this.editForm.value['tags'];
      article.content = this.editForm.value['content'];
      if (this.currentId) {
        article.id = this.currentId;
        this._articleService.update(article).subscribe(result => {
          this._messageService.success('文章更新成功！');
          this._router.navigate(['/content/article']);
        });
      } else {
        this._articleService.add(article).subscribe(result => {
          this._messageService.success('文章创建成功！');
          this._router.navigate(['/content/article']);
        });
      }
    }
  }

  back() {
    this._router.navigate(['/content/article']);
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

}
