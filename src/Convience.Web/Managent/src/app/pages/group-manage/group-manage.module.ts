import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PositionManageComponent } from './position-manage/position-manage.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { DepartmentManageComponent } from './department-manage/department-manage.component';
import { LoginGuard } from 'src/app/guards/login.guard';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzTreeModule } from 'ng-zorro-antd/tree';
import { NzTreeSelectModule } from 'ng-zorro-antd/tree-select';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    PositionManageComponent,
    DepartmentManageComponent,
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    SharedModule,
    RouterModule.forChild([
      { path: 'position', component: PositionManageComponent, canActivate: [LoginGuard] },
      { path: 'department', component: DepartmentManageComponent, canActivate: [LoginGuard] },
    ]),
    
    // NGZorro组件
    NzFormModule,
    NzSelectModule,
    NzCardModule,
    NzInputModule,
    NzTableModule,
    NzTreeSelectModule,
    NzTreeModule,
    NzPaginationModule,
    NzTagModule,
    NzAvatarModule,
    NzIconModule,
    NzButtonModule
  ]
})
export class GroupManageModule { }
