import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';


@NgModule({
  declarations: [LayoutComponent],
  imports: [
    CommonModule,
    RouterModule,

    MatToolbarModule,
    MatIconModule,
    MatButtonModule
  ]
})
export class LayoutModule { }
