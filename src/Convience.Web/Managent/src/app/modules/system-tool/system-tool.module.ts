import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginGuard } from 'src/app/core/guards/login.guard';
import { SwaggerUiComponent } from './swagger-ui/swagger-ui.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { HangfireUIComponent } from './hangfire-ui/hangfire-ui.component';



@NgModule({
  declarations: [
    SwaggerUiComponent,
    HangfireUIComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'swagger', component: SwaggerUiComponent, canActivate: [LoginGuard] },
      { path: 'hangfire', component: HangfireUIComponent, canActivate: [LoginGuard] },
    ])
  ]
})
export class SystemToolModule { }
