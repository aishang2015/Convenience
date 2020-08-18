import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from './pages/layout/layout.module';
import { AccountModule } from './pages/account/account.module';
import { HomeModule } from './pages/home/home.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthHeaderInterceptor } from '../app/inceptor/auth-header-inceptor';
import { ErrorHandlerInterceptor } from '../app/inceptor/error-handler-inceptor';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule, 
    ToastrModule.forRoot(),

    LayoutModule,
    AccountModule,
    HomeModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHeaderInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorHandlerInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
