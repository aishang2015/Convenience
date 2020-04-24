import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileManageComponent } from './file-manage/file-manage.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { RouterModule } from '@angular/router';
import { LoginGuard } from 'src/app/core/guards/login.guard';
import { FileIconComponent } from './file-icon/file-icon.component';
import { NzUploadModule } from 'ng-zorro-antd';

@NgModule({
  declarations: [
    FileManageComponent,
    FileIconComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    NzUploadModule,
    RouterModule.forChild([
      { path: 'file', component: FileManageComponent, canActivate: [LoginGuard] }
    ])
  ]
})
export class ContentManageModule { }
