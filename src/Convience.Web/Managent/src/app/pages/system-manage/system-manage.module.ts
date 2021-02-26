import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppCommonModule } from '../app-common/app-common.module';
import { RouterModule } from '@angular/router';
import { RoleManageComponent } from './role-manage/role-manage.component';
import { UserManageComponent } from './user-manage/user-manage.component';
import { MenuManageComponent } from './menu-manage/menu-manage.component';
import { LoginGuard } from 'src/app/guards/login.guard';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzTreeModule } from 'ng-zorro-antd/tree';
import { NzTreeSelectModule } from 'ng-zorro-antd/tree-select';
import { AvatarSelectComponent } from './avatar-select/avatar-select.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    RoleManageComponent,
    UserManageComponent,
    MenuManageComponent,
    AvatarSelectComponent,
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    SharedModule,
    RouterModule.forChild([
      { path: 'role', component: RoleManageComponent, canActivate: [LoginGuard] },
      { path: 'user', component: UserManageComponent, canActivate: [LoginGuard] },
      { path: 'menu', component: MenuManageComponent, canActivate: [LoginGuard] },
    ]),

    // NGZorro组件
    NzSelectModule,
    NzFormModule,
    NzButtonModule,
    NzPaginationModule,
    NzTableModule,
    NzTagModule,
    NzAvatarModule,
    NzCardModule,
    NzInputModule,
    NzTreeModule,
    NzTreeSelectModule,
    NzIconModule,
    NzRadioModule,
    NzSwitchModule
  ]
})
export class SystemManageModule { }
