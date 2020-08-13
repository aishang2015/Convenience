import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './pages/layout/layout/layout.component';


const routes: Routes = [
  { path: "account", loadChildren: () => import("./pages/account/account.module").then(m => m.AccountModule) },
  {
    path: "", component: LayoutComponent, children: [
      { path: "", loadChildren: () => import("./pages/infomation/infomation.module").then(m => m.InfomationModule) },
      { path: "system", loadChildren: () => import("./pages/system-manage/system-manage.module").then(m => m.SystemManageModule) },
      { path: "group", loadChildren: () => import("./pages/group-manage/group-manage.module").then(m => m.GroupManageModule) },
      { path: "content", loadChildren: () => import("./pages/content-manage/content-manage.module").then(m => m.ContentManageModule) },
      { path: "saas", loadChildren: () => import("./pages/saas-manage/saas-manage.module").then(m => m.SaasManageModule) },
      { path: "tool", loadChildren: () => import("./pages/system-tool/system-tool.module").then(m => m.SystemToolModule) },
      { path: "workflow", loadChildren: () => import("./pages/workflow/workflow.module").then(m => m.WorkflowModule) }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
