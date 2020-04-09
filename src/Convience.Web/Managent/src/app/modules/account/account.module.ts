import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { LoginGuard } from '../../core/guards/login.guard';
import { AppCommonModule } from '../app-common/app-common.module';
import { ModifyPasswordComponent } from './modify-password/modify-password.component';

@NgModule({
  declarations: [
    LoginComponent,
    ModifyPasswordComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: '', pathMatch: 'full', redirectTo: 'login' },
      { path: "login", component: LoginComponent, canActivate: [LoginGuard] },      
      { path: "changePwd", component: ModifyPasswordComponent, canActivate: [LoginGuard] }
    ])
  ]
})
export class AccountModule { }
