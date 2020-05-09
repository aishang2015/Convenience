import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ArticleService } from 'src/app/services/article.service';
import { ColumnService } from 'src/app/services/column.service';
import { Column } from '../model/column';
import { NzTreeNodeOptions, NzMessageService } from 'ng-zorro-antd';
import { Article } from '../model/article';
import { StorageService } from 'src/app/core/services/storage.service';

@Component({
  selector: 'app-article-edit',
  templateUrl: './article-edit.component.html',
  styleUrls: ['./article-edit.component.scss']
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

  constructor(private route: ActivatedRoute,
    private router: Router,
    private articleService: ArticleService,
    private messageService: NzMessageService,
    private columnService: ColumnService,
    private formBuilder: FormBuilder,
    private storageService: StorageService) { }

  ngOnInit(): void {
    this.storageService.clearTinymceCache();
    this.editForm = this.formBuilder.group({
      title: [null, [Validators.required, Validators.maxLength(50)]],
      subTitle: [null, [Validators.maxLength(200)]],
      columnId: [null],
      source: [null, [Validators.maxLength(200)]],
      sort: [null, [Validators.required]],
      tags: [null, [Validators.maxLength(200)]],
      content: [null]
    });
    let id = this.route.snapshot.paramMap.get('id')?.trim();
    this.currentId = id ? Number(id) : null;
    if (this.currentId) {
      this.articleService.get(this.currentId).subscribe((result: any) => {
        this.editForm = this.formBuilder.group({
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
        this.articleService.update(article).subscribe(result => {
          this.messageService.success('文章更新成功！');
          this.router.navigate(['/content/article']);
        });
      } else {
        this.articleService.add(article).subscribe(result => {
          this.messageService.success('文章创建成功！');
          this.router.navigate(['/content/article']);
        });
      }
    }
  }

  back() {
    this.router.navigate(['/content/article']);
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

}
