import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppCommonModule } from '../common/modules/app-common/app-common.module';
import { RouterModule } from '@angular/router';
import { RoleManageComponent } from './role-manage/role-manage.component';
import { LoginGuard } from '../core/guards/login.guard';



@NgModule({
  declarations: [
    RoleManageComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'role', component: RoleManageComponent, canActivate: [LoginGuard] },
    ])
  ]
})
export class SystemManageModule { }
