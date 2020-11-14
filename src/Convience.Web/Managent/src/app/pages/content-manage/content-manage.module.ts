import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileManageComponent } from './file-manage/file-manage.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { RouterModule } from '@angular/router';
import { FileIconComponent } from './file-icon/file-icon.component';
import { ArticleManageComponent } from './article-manage/article-manage.component';
import { ArticleEditComponent } from './article-edit/article-edit.component';
import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { DicManageComponent } from './dic-manage/dic-manage.component';
import { LoginGuard } from 'src/app/guards/login.guard';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzTreeModule } from 'ng-zorro-antd/tree';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzTreeSelectModule } from 'ng-zorro-antd/tree-select';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';

@NgModule({
  declarations: [
    FileManageComponent,
    FileIconComponent,
    ArticleManageComponent,
    ArticleEditComponent,
    DicManageComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    NzUploadModule,
    RouterModule.forChild([
      { path: 'file', component: FileManageComponent, canActivate: [LoginGuard] },
      { path: 'article', component: ArticleManageComponent, canActivate: [LoginGuard] },
      { path: 'article/edit', component: ArticleEditComponent, canActivate: [LoginGuard] },
      { path: 'dic', component: DicManageComponent, canActivate: [LoginGuard] },

    ]),
    EditorModule,

    // NGZorro组件
    NzAlertModule,
    NzListModule,
    NzFormModule,
    NzInputModule,
    NzTableModule,
    NzPaginationModule,
    NzButtonModule,
    NzCardModule,
    NzTreeSelectModule,
    NzTreeModule,
    NzTagModule,
    NzBreadCrumbModule,
    NzIconModule,

  ],
  providers: [
    { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' }
  ]
})
export class ContentManageModule { }
