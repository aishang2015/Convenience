import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AppCommonModule } from '../app-common/app-common.module';
import { CodeGeneratorComponent } from './code-generator/code-generator.component';
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
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzCodeEditorModule } from 'ng-zorro-antd/code-editor';


@NgModule({
  declarations: [
    CodeGeneratorComponent,
    ManageUiComponent,
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
export class SystemToolModule { }
