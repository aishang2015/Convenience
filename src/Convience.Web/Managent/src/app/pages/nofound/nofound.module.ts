import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NofoundComponent } from './nofound.component';
import { RouterModule } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';



@NgModule({
  declarations: [NofoundComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', component: NofoundComponent }
    ]),

    // NGZorro组件
    NzIconModule,
    NzButtonModule
  ],
  exports: [
    NofoundComponent,
  ]
})
export class NofoundModule { }
