import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PositionManageComponent } from './position-manage/position-manage.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { DepartmentTreeComponent } from './department-tree/department-tree.component';
import { DepartmentManageComponent } from './department-manage/department-manage.component';
import { EmployeeComponent } from './employee/employee.component';
import { LoginGuard } from 'src/app/guards/login.guard';
import { NzFormModule, NzSelectModule, NzCardModule, NzInputModule, NzTableModule, NzTreeSelectModule, NzTreeModule, NzPaginationModule, NzTagModule, NzAvatarModule, NzIconModule, NzButtonModule } from 'ng-zorro-antd';



@NgModule({
  declarations: [
    PositionManageComponent,
    DepartmentTreeComponent,
    DepartmentManageComponent,
    EmployeeComponent,
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'position', component: PositionManageComponent, canActivate: [LoginGuard] },
      { path: 'department', component: DepartmentManageComponent, canActivate: [LoginGuard] },
      { path: 'employee', component: EmployeeComponent, canActivate: [LoginGuard] },
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
