
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { CommonModule } from '@angular/common';
import { GenderPipe } from './pipes/gender.pipe';
import { AvatarSelectComponent } from './components/avatar-select/avatar-select.component';

@NgModule({
  declarations: [
    GenderPipe,
    AvatarSelectComponent,
  ],
  imports:[
    CommonModule,
    NgZorroAntdModule,
  ],
  exports: [
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule, 
    CommonModule,
    NgZorroAntdModule,
    GenderPipe,
    AvatarSelectComponent,
  ]
})
export class AppCommonModule { }
