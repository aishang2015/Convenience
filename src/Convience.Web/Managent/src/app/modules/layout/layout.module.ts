import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout/layout.component';
import { NzBreadCrumbModule, NzLayoutModule, NzDropDownModule, NzCardModule, NzAvatarModule, NzBadgeModule, NzIconModule, NzMessageModule } from 'ng-zorro-antd';
import { FormsModule } from '@angular/forms';
import { AppCommonModule } from 'src/app/modules/app-common/app-common.module';
import { RouterModule } from '@angular/router';
import { CanOperateDirective } from 'src/app/modules/app-common/directives/can-operate.directive';



@NgModule({
  declarations: [
    LayoutComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
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
