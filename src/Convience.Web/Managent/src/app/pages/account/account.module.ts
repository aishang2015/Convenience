import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { NzCardModule, NzFormModule, NzButtonModule, NzIconModule, NzInputModule } from 'ng-zorro-antd';
import { AuthGuard } from 'src/app/guards/auth.guard';

@NgModule({
  declarations: [
    LoginComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: '', pathMatch: 'full', redirectTo: 'login' },
      { path: "login", component: LoginComponent, canActivate: [AuthGuard] }
    ]),

    // NGZorro组件
    NzFormModule,
    NzCardModule,
    NzButtonModule,
    NzIconModule,
    NzFormModule,
    NzInputModule,
  ]
})
export class AccountModule { }
