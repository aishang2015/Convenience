import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppCommonModule } from '../app-common/app-common.module';
import { RouterModule } from '@angular/router';
import { TenantManageComponent } from './tenant-manage/tenant-manage.component';
import { LoginGuard } from 'src/app/guards/login.guard';
import { NzSelectModule, NzFormModule, NzButtonModule, NzPaginationModule, NzTagModule, NzTableModule, NzRadioModule, NzCardModule, NzInputModule } from 'ng-zorro-antd';



@NgModule({
  declarations: [
    TenantManageComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'tenant', component: TenantManageComponent, canActivate: [LoginGuard] },
    ]),

    // NGZorro组件
    NzSelectModule,
    NzFormModule,
    NzButtonModule,
    NzPaginationModule,
    NzTagModule,
    NzTableModule,
    NzRadioModule,
    NzCardModule,
    NzInputModule
  ]
})
export class SaasManageModule { }
