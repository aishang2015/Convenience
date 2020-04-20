import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { AuthHeaderInterceptor } from './core/inceptors/auth-header-inceptor';
import { CacheInterceptor } from './core/inceptors/cache-inceptor';
import { ErrorHandlerInterceptor } from './core/inceptors/error-handler-inceptor';
import { UriConstant } from './core/constants/uri-constant';
import { LayoutModule } from './modules/layout/layout.module';

export function initializeApp(uriConstant: UriConstant) {
  return () => uriConstant.init();
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    LayoutModule,
  ],
  bootstrap: [AppComponent],
  /** 配置 ng-zorro-antd 国际化（文案 及 日期） **/
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHeaderInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: CacheInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorHandlerInterceptor, multi: true },

    // 在uriconstant被输入容器之前，执行指定的promise，程序会阻塞直到promise resolve
    UriConstant,
    { provide: APP_INITIALIZER, useFactory: initializeApp, deps: [UriConstant], multi: true }
  ]
})
export class AppModule { }