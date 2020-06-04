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



@NgModule({
  declarations: [
    FormDesignComponent,
    FlowDesignComponent,
    MyFlowComponent,
    WorkflowManageComponent
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
    DragDropModule
  ]
})
export class WorkflowModule { }
