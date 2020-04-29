import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginGuard } from 'src/app/core/guards/login.guard';
import { SwaggerUiComponent } from './swagger-ui/swagger-ui.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { HangfireUIComponent } from './hangfire-ui/hangfire-ui.component';
import { CapUiComponent } from './cap-ui/cap-ui.component';
import { CodeGeneratorComponent } from './code-generator/code-generator.component';
import { NzStepsModule, NzTabsModule, NzRadioModule, NzResultModule } from 'ng-zorro-antd';
import { MonacoEditorModule } from 'ngx-monaco-editor';



@NgModule({
  declarations: [
    SwaggerUiComponent,
    HangfireUIComponent,
    CapUiComponent,
    CodeGeneratorComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'swagger', component: SwaggerUiComponent, canActivate: [LoginGuard] },
      { path: 'hangfire', component: HangfireUIComponent, canActivate: [LoginGuard] },
      { path: 'cap', component: CapUiComponent, canActivate: [LoginGuard] },
      { path: 'code', component: CodeGeneratorComponent, canActivate: [LoginGuard] },
    ]),
    NzStepsModule,
    MonacoEditorModule.forRoot(),
    NzRadioModule,
    NzResultModule
  ]
})
export class SystemToolModule { }
