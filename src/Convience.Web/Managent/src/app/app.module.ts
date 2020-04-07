import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { LayoutComponent } from './common/components/layout/layout.component';
import { AuthHeaderInterceptor } from './core/inceptors/auth-header-inceptor';
import { CacheInterceptor } from './core/inceptors/cache-inceptor';
import { ErrorHandlerInterceptor } from './core/inceptors/error-handler-inceptor';
import { OperateDirective } from './common/directives/operate.directive';
import { UriConstant } from './common/constants/uri-constant';
import { NzBreadCrumbModule, NzLayoutModule, NzDropDownModule, NzCardModule, NzAvatarModule, NzIconModule, NzMessageModule } from 'ng-zorro-antd';
import { NzBadgeModule } from 'ng-zorro-antd/badge';

export function initializeApp(uriConstant: UriConstant) {
  return () => uriConstant.init();
}

@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent,
    OperateDirective
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NzBreadCrumbModule,
    NzLayoutModule,
    NzDropDownModule,
    NzCardModule,
    NzAvatarModule,
    NzBadgeModule,
    NzIconModule,
    NzMessageModule
  ],
  bootstrap: [AppComponent],
  /** 配置 ng-zorro-antd 国际化（文案 及 日期） **/
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHeaderInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: CacheInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorHandlerInterceptor, multi: true },
    UriConstant,

    // 在uriconstant被输入容器之前，执行指定的promise，程序会阻塞直到promise resolve
    { provide: APP_INITIALIZER, useFactory: initializeApp, deps: [UriConstant], multi: true }
  ]
})
export class AppModule { }