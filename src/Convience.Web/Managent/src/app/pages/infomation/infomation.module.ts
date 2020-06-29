import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RouterModule } from '@angular/router';
import { NzStatisticModule } from 'ng-zorro-antd';
import { LoginGuard } from 'src/app/guards/login.guard';
import { AppCommonModule } from '../app-common/app-common.module';



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
