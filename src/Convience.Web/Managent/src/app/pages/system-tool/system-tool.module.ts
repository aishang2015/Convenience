import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AppCommonModule } from '../app-common/app-common.module';
import { CodeGeneratorComponent } from './code-generator/code-generator.component';
import { NzStepsModule, NzTabsModule, NzRadioModule, NzResultModule, NzFormModule, NzPaginationModule, NzSelectModule, NzCardModule, NzIconModule, NzInputModule, NzButtonModule, NzSwitchModule } from 'ng-zorro-antd';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { LoginGuard } from 'src/app/guards/login.guard';
import { ManageUiComponent } from './manage-ui/manage-ui.component';



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
      { path: 'hangfire', component: ManageUiComponent, data: { uri: 'hangfire' }, canActivate: [LoginGuard] },
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
