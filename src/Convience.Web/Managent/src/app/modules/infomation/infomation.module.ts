import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RouterModule } from '@angular/router';
import { LoginGuard } from '../../core/guards/login.guard';
import { AppCommonModule } from 'src/app/modules/app-common/app-common.module';
import { NzStatisticModule } from 'ng-zorro-antd';



@NgModule({
  declarations: [
    DashboardComponent,
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      { path: 'dashboard', component: DashboardComponent, canActivate: [LoginGuard] }
    ]),
    NzStatisticModule
  ]
})
export class InfomationModule { }
