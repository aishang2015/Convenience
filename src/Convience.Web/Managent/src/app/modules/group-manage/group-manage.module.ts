import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginGuard } from 'src/app/core/guards/login.guard';
import { PositionManageComponent } from './position-manage/position-manage.component';
import { AppCommonModule } from '../app-common/app-common.module';



@NgModule({
  declarations: [PositionManageComponent],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'position', component: PositionManageComponent, canActivate: [LoginGuard] },
      { path: 'department', canActivate: [LoginGuard] },
      { path: 'employee', canActivate: [LoginGuard] },
    ])
  ]
})
export class GroupManageModule { }
