import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginGuard } from './core/guards/login.guard';


const routes: Routes = [
  { path: "", loadChildren: () => import("./modules/infomation/infomation.module").then(m => m.InfomationModule) },
  { path: "account", loadChildren: () => import("./modules/account/account.module").then(m => m.AccountModule) },
  { path: "system", loadChildren: () => import("./modules/system-manage/system-manage.module").then(m => m.SystemManageModule) },
  { path: "group", loadChildren: () => import("./modules/group-manage/group-manage.module").then(m => m.GroupManageModule) },
  { path: "content", loadChildren: () => import("./modules/content-manage/content-manage.module").then(m => m.ContentManageModule) },
  { path: "saas", loadChildren: () => import("./modules/saas-manage/saas-manage.module").then(m => m.SaasManageModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
