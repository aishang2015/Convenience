import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { zh_CN } from 'ng-zorro-antd/i18n';
import { DatePipe, registerLocaleData } from '@angular/common';
import zh from '@angular/common/locales/zh';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from './pages/layout/layout.module';
import { AuthHeaderInterceptor } from './inceptors/auth-header-inceptor';
import { CacheInterceptor } from './inceptors/cache-inceptor';
import { ErrorHandlerInterceptor } from './inceptors/error-handler-inceptor';
import { UriConfig } from './configs/uri-config';

registerLocaleData(zh);

export function initializeApp(uriConstant: UriConfig) {
  return () => uriConstant.init();
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    LayoutModule,
  ],
  bootstrap: [AppComponent],
  /** 配置 ng-zorro-antd 国际化（文案 及 日期） **/
  providers: [
    { provide: NZ_I18N, useValue: zh_CN },
    { provide: HTTP_INTERCEPTORS, useClass: AuthHeaderInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: CacheInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorHandlerInterceptor, multi: true },

    // 在uriconstant被输入容器之前，执行指定的promise，程序会阻塞直到promise resolve
    UriConfig,
    { provide: APP_INITIALIZER, useFactory: initializeApp, deps: [UriConfig], multi: true },
    DatePipe
  ]
})
export class AppModule { }
