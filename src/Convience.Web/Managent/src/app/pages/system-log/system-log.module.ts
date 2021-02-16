import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OperateLogManageComponent } from './operate-log-manage/operate-log-manage.component';
import { OperateLogViewComponent } from './operate-log-view/operate-log-view.component';
import { RouterModule } from '@angular/router';
import { AppCommonModule } from '../app-common/app-common.module';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzCodeEditorModule } from 'ng-zorro-antd/code-editor';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzResultModule } from 'ng-zorro-antd/result';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzStepsModule } from 'ng-zorro-antd/steps';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { LoginGuard } from 'src/app/guards/login.guard';
import { LoginLogViewComponent } from './login-log-view/login-log-view.component';



@NgModule({
  declarations: [
    OperateLogManageComponent,
    OperateLogViewComponent,
    LoginLogViewComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'operate', component: OperateLogViewComponent, canActivate: [LoginGuard] },
      { path: 'login', component: LoginLogViewComponent, canActivate: [LoginGuard] },
    ]),

    // NGZorro组件
    NzStepsModule,
    NzRadioModule,
    NzResultModule,
    NzFormModule,
    NzInputModule,
    NzPaginationModule,
    NzSelectModule,
    NzCardModule,
    NzIconModule,
    NzButtonModule,
    NzSwitchModule,
    NzTableModule,
    NzInputNumberModule,
    NzTagModule,
    NzDatePickerModule,
    NzCodeEditorModule
  ]
})
export class SystemLogModule { }
