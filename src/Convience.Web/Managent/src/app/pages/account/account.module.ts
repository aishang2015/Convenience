import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { LoginGuard } from 'src/app/guards/login.guard';

@NgModule({
  declarations: [
    LoginComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: '', pathMatch: 'full', redirectTo: 'login' },
      { path: "login", component: LoginComponent, canActivate: [LoginGuard] }
    ])
  ]
})
export class AccountModule { }
