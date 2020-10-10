import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppCommonModule } from '../app-common/app-common.module';
import { RouterModule } from '@angular/router';
import { TenantManageComponent } from './tenant-manage/tenant-manage.component';
import { LoginGuard } from 'src/app/guards/login.guard';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzInputModule } from 'ng-zorro-antd/input';



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
    NzInputModule,
    NzSwitchModule,
    NzIconModule
  ]
})
export class SaasManageModule { }
