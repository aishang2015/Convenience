
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NzSelectModule, NzFormModule, NzCardModule, NzRadioModule, NzTableModule, NzTagModule, NzPaginationModule, NzInputModule, NzTreeModule, NzAvatarModule, NzTreeSelectModule, NzButtonModule, NzIconModule, NzModalModule, NzSwitchModule, NzBreadCrumbModule } from 'ng-zorro-antd';
import { CommonModule } from '@angular/common';
import { AvatarSelectComponent } from './components/avatar-select/avatar-select.component';
import { GenderPipe } from 'src/app/pipes/gender.pipe';
import { DbTypePipe } from 'src/app/pipes/db-type.pipe';
import { MenuTypePipe } from 'src/app/pipes/menu-type.pipe';
import { CanOperateDirective } from 'src/app/directives/can-operate.directive';

@NgModule({
  declarations: [
    GenderPipe,
    MenuTypePipe,
    DbTypePipe,
    AvatarSelectComponent,
    CanOperateDirective
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,

    // NGZorro组件
    NzSelectModule,
    NzFormModule,
    NzCardModule,
    NzRadioModule,
    NzTableModule,
    NzTagModule,
    NzPaginationModule,
    NzInputModule,
    NzTreeModule,
    NzAvatarModule,
    NzTreeSelectModule,
    NzButtonModule,
    NzIconModule,
    NzModalModule,
    NzSwitchModule,
    NzBreadCrumbModule,

    // 管道
    GenderPipe,
    MenuTypePipe,
    DbTypePipe,

    // 组件
    AvatarSelectComponent,

    // 指令
    CanOperateDirective
  ]
})
export class AppCommonModule { }
