import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormDesignComponent } from './form-design/form-design.component';
import { MyFlowComponent } from './my-flow/my-flow.component';
import { WorkflowManageComponent } from './workflow-manage/workflow-manage.component';
import { FlowDesignComponent } from './flow-design/flow-design.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { RouterModule } from '@angular/router';
import { LoginGuard } from 'src/app/core/guards/login.guard';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzTimePickerModule } from 'ng-zorro-antd/time-picker';
import { registerLocaleData } from '@angular/common';
import zh from '@angular/common/locales/zh';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzDividerModule } from 'ng-zorro-antd';
import { WorkflowGroupTreeComponent } from './workflow-group-tree/workflow-group-tree.component';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
registerLocaleData(zh);



@NgModule({
  declarations: [
    FormDesignComponent,
    FlowDesignComponent,
    MyFlowComponent,
    WorkflowManageComponent,
    WorkflowGroupTreeComponent,
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'flowDesign', component: FlowDesignComponent, canActivate: [LoginGuard] },
      { path: 'formDesign', component: FormDesignComponent, canActivate: [LoginGuard] },
      { path: 'workflowManage', component: WorkflowManageComponent, canActivate: [LoginGuard] },
      { path: 'myFlow', component: MyFlowComponent, canActivate: [LoginGuard] },
    ]),
    DragDropModule,
    NzDatePickerModule,
    NzInputNumberModule,
    NzTimePickerModule,
    NzSwitchModule,
    NzDividerModule,
    NzToolTipModule
  ]
})
export class WorkflowModule { }
