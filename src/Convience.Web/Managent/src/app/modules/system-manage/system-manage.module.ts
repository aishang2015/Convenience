import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppCommonModule } from '../app-common/app-common.module';
import { RouterModule } from '@angular/router';
import { RoleManageComponent } from './role-manage/role-manage.component';
import { LoginGuard } from '../../core/guards/login.guard';
import { UserManageComponent } from './user-manage/user-manage.component';
import { MenuManageComponent } from './menu-manage/menu-manage.component';

@NgModule({
  declarations: [
    RoleManageComponent,
    UserManageComponent,
    MenuManageComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'role', component: RoleManageComponent, canActivate: [LoginGuard] },
      { path: 'user', component: UserManageComponent, canActivate: [LoginGuard] },
      { path: 'menu', component: MenuManageComponent, canActivate: [LoginGuard] },
    ])
  ]
})
export class SystemManageModule { }
