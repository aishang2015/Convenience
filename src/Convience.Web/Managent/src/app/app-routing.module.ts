import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './pages/layout/layout/layout.component';

const routes: Routes = [
  { path: "account", loadChildren: () => import("./pages/account/account.module").then(m => m.AccountModule) },
  {
    path: "", component: LayoutComponent, children: [
      { path: "", loadChildren: () => import("./pages/infomation/infomation.module").then(m => m.InfomationModule) },
      { path: "system", loadChildren: () => import("./pages/system-manage/system-manage.module").then(m => m.SystemManageModule) },
      { path: "group", loadChildren: () => import("./pages/group-manage/group-manage.module").then(m => m.GroupManageModule) },
      { path: "content", loadChildren: () => import("./pages/content-manage/content-manage.module").then(m => m.ContentManageModule) },
      { path: "tool", loadChildren: () => import("./pages/system-tool/system-tool.module").then(m => m.SystemToolModule) },
      { path: "workflow", loadChildren: () => import("./pages/workflow/workflow.module").then(m => m.WorkflowModule) },
      { path: "log", loadChildren: () => import("./pages/system-log/system-log.module").then(m => m.SystemLogModule) },
    ]
  },
  { path: "**", loadChildren: () => import("./pages/nofound/nofound.module").then(m => m.NofoundModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
