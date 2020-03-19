import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginGuard } from './core/guards/login.guard';


const routes: Routes = [
  { path: "", loadChildren: () => import("./infomation/infomation.module").then(m => m.InfomationModule) },
  { path: "account", loadChildren: () => import("./account/account.module").then(m => m.AccountModule) },
  { path: "system", loadChildren: () => import("./system-manage/system-manage.module").then(m => m.SystemManageModule) },
  { path: "group", loadChildren: () => import("./group-manage/group-manage.module").then(m => m.GroupManageModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
