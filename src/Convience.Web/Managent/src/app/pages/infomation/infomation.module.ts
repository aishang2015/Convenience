import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RouterModule } from '@angular/router';
import { NzStatisticModule, NzLayoutModule, NzCardModule, NzGridModule, NzInputModule, NzModalModule } from 'ng-zorro-antd';
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

    // NGZorro组件
    NzStatisticModule,
    NzCardModule,
    NzGridModule
  ]
})
export class InfomationModule { }
