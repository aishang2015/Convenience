
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AvatarSelectComponent } from './components/avatar-select/avatar-select.component';
import { GenderPipe } from 'src/app/pipes/gender.pipe';
import { DbTypePipe } from 'src/app/pipes/db-type.pipe';
import { MenuTypePipe } from 'src/app/pipes/menu-type.pipe';
import { CanOperateDirective } from 'src/app/directives/can-operate.directive';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { DepartmentTreeComponent } from './components/department-tree/department-tree.component';
import { NzTreeModule } from 'ng-zorro-antd/tree';
import { NzIconModule } from 'ng-zorro-antd/icon';

@NgModule({
  declarations: [
    GenderPipe,
    MenuTypePipe,
    DbTypePipe,
    AvatarSelectComponent,
    DepartmentTreeComponent,
    CanOperateDirective,
  ],
  imports: [
    CommonModule,
    NzButtonModule,
    NzTreeModule,
    NzIconModule
  ],
  exports: [
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,

    // 管道
    GenderPipe,
    MenuTypePipe,
    DbTypePipe,

    // 组件
    AvatarSelectComponent,
    DepartmentTreeComponent,

    // 指令
    CanOperateDirective
  ]
})
export class AppCommonModule { }
