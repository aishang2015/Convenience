import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { UriConfig } from './configs/uri-config';
import { AuthHeaderInterceptor } from './inceptors/auth-header-inceptor';
import { CacheInterceptor } from './inceptors/cache-inceptor';
import { ErrorHandlerInterceptor } from './inceptors/error-handler-inceptor';
import { LayoutModule } from './pages/layout/layout.module';


export function initializeApp(uriConstant: UriConfig) {
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
    UriConfig,
    { provide: APP_INITIALIZER, useFactory: initializeApp, deps: [UriConfig], multi: true }
  ]
})
export class AppModule { }