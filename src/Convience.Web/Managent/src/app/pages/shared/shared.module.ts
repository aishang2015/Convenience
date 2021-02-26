import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DepartmentTreeComponent } from './components/department-tree/department-tree.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTreeModule } from 'ng-zorro-antd/tree';



@NgModule({
  declarations: [
    DepartmentTreeComponent
  ],
  imports: [
    CommonModule,
    NzButtonModule,
    NzTreeModule,
    NzIconModule
  ],
  exports:[
    DepartmentTreeComponent
  ]
})
export class SharedModule { }
