import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AppCommonModule } from '../app-common/app-common.module';
import { CodeGeneratorComponent } from './code-generator/code-generator.component';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { LoginGuard } from 'src/app/guards/login.guard';
import { ManageUiComponent } from './manage-ui/manage-ui.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzStepsModule } from 'ng-zorro-antd/steps';
import { NzResultModule } from 'ng-zorro-antd/result';


@NgModule({
  declarations: [
    CodeGeneratorComponent,
    ManageUiComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'swagger', component: ManageUiComponent, data: { uri: 'swagger/index.html' }, canActivate: [LoginGuard] },
      { path: 'hangfire', component: ManageUiComponent, data: { uri: 'taskview' }, canActivate: [LoginGuard] },
      { path: 'cap', component: ManageUiComponent, data: { uri: 'cap' }, canActivate: [LoginGuard] },
      { path: 'code', component: CodeGeneratorComponent, canActivate: [LoginGuard] },
    ]),
    MonacoEditorModule.forRoot(),

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
    NzSwitchModule
  ]
})
export class SystemToolModule { }
