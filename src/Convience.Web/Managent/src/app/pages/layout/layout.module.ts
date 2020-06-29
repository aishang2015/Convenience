import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout/layout.component';
import { NzBreadCrumbModule, NzLayoutModule, NzDropDownModule, NzCardModule, NzAvatarModule, NzBadgeModule, NzIconModule, NzMessageModule } from 'ng-zorro-antd';
import { RouterModule } from '@angular/router';
import { AppCommonModule } from '../app-common/app-common.module';



@NgModule({
  declarations: [
    LayoutComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    AppCommonModule,
    NzBreadCrumbModule,
    NzLayoutModule,
    NzDropDownModule,
    NzCardModule,
    NzAvatarModule,
    NzBadgeModule,
    NzIconModule,
    NzMessageModule
  ],
  exports: [
    LayoutComponent
  ]
})
export class LayoutModule { }
