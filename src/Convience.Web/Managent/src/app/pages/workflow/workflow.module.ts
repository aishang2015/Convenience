import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormDesignComponent } from './form-design/form-design.component';
import { MyFlowComponent } from './my-flow/my-flow.component';
import { WorkflowManageComponent } from './workflow-manage/workflow-manage.component';
import { FlowDesignComponent } from './flow-design/flow-design.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { RouterModule } from '@angular/router';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzTimePickerModule } from 'ng-zorro-antd/time-picker';
import { registerLocaleData } from '@angular/common';
import zh from '@angular/common/locales/zh';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { WorkflowGroupTreeComponent } from './workflow-group-tree/workflow-group-tree.component';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { HandleWorkFlowComponent } from './handle-work-flow/handle-work-flow.component';
import { LoginGuard } from 'src/app/guards/login.guard';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTreeModule } from 'ng-zorro-antd/tree';
import { NzTreeSelectModule } from 'ng-zorro-antd/tree-select';
import { NzDividerModule } from 'ng-zorro-antd/divider';
registerLocaleData(zh);



@NgModule({
  declarations: [
    FormDesignComponent,
    FlowDesignComponent,
    MyFlowComponent,
    WorkflowManageComponent,
    WorkflowGroupTreeComponent,
    HandleWorkFlowComponent,
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'flowDesign', component: FlowDesignComponent, canActivate: [LoginGuard] },
      { path: 'formDesign', component: FormDesignComponent, canActivate: [LoginGuard] },
      { path: 'workflowManage', component: WorkflowManageComponent, canActivate: [LoginGuard] },
      { path: 'myFlow', component: MyFlowComponent, canActivate: [LoginGuard] },
      { path: 'handledFlow', component: HandleWorkFlowComponent, canActivate: [LoginGuard] },
    ]),

    // NGZorro组件
    NzSelectModule,
    NzFormModule,
    NzDatePickerModule,
    NzInputNumberModule,
    NzTimePickerModule,
    NzSwitchModule,
    NzDividerModule,
    NzToolTipModule,
    NzTreeModule,
    NzTreeSelectModule,
    NzTableModule,
    NzButtonModule,
    NzPaginationModule,
    NzCardModule,
    NzInputModule,
    NzIconModule,
  ]
})
export class WorkflowModule { }
