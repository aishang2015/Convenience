import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';

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
