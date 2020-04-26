import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ArticleService } from 'src/app/services/article.service';
import { ColumnService } from 'src/app/services/column.service';
import { Column } from '../model/column';
import { NzTreeNodeOptions } from 'ng-zorro-antd';

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

  constructor(private route: ActivatedRoute,
    private router: Router,
    private articleService: ArticleService,
    private columnService: ColumnService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    let id = this.route.snapshot.paramMap.get('id');
    this.currentId = id ? Number(id) : null;
    if (this.currentId) {
      this.articleService.get(this.currentId).subscribe((result: any) => {
        this.editForm = this.formBuilder.group({
          title: [result['title'], [Validators.required, Validators.maxLength(50)]],
          subTitle: [result['subTitle'], [Validators.maxLength(200)]],
          columnId: [result['columnId']],
          source: [result['source'], [Validators.maxLength(200)]],
          sort: [result['sort']],
          tags: [result['tags'], [Validators.maxLength(200)]],
          content: [result['content']]
        });
      });
    } else {
      this.editForm = this.formBuilder.group({
        title: [null, [Validators.required, Validators.maxLength(50)]],
        subTitle: [null, [Validators.maxLength(200)]],
        columnId: [null],
        source: [null, [Validators.maxLength(200)]],
        sort: [null],
        tags: [null, [Validators.maxLength(200)]],
        content: [null]
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
      if (this.currentId) {

      } else {

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
