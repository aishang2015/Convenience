import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './pages/layout/layout/layout.component';


const routes: Routes = [
  { path: "account", loadChildren: () => import("./pages/account/account.module").then(m => m.AccountModule) },
  { path: "", component: LayoutComponent, loadChildren: () => import("./pages/infomation/infomation.module").then(m => m.InfomationModule) },
  { path: "system", component: LayoutComponent, loadChildren: () => import("./pages/system-manage/system-manage.module").then(m => m.SystemManageModule) },
  { path: "group", component: LayoutComponent, loadChildren: () => import("./pages/group-manage/group-manage.module").then(m => m.GroupManageModule) },
  { path: "content", component: LayoutComponent, loadChildren: () => import("./pages/content-manage/content-manage.module").then(m => m.ContentManageModule) },
  { path: "saas", component: LayoutComponent, loadChildren: () => import("./pages/saas-manage/saas-manage.module").then(m => m.SaasManageModule) },
  { path: "tool", component: LayoutComponent, loadChildren: () => import("./pages/system-tool/system-tool.module").then(m => m.SystemToolModule) },
  { path: "workflow", component: LayoutComponent, loadChildren: () => import("./pages/workflow/workflow.module").then(m => m.WorkflowModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
