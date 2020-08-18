﻿using Convience.Entity.Data;
using Convience.Entity.Entity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace Convience.ManagentApi.Infrastructure
{
    public class DbContextSeed
    {
        public static void InitialApplicationDataBase(SystemIdentityDbContext dbContext,
            IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<SystemUser>>();
            var roleManager = services.GetRequiredService<RoleManager<SystemRole>>();
            roleManager.CreateAsync(new SystemRole
            {
                Name = "超级管理员",
                Remark = "系统超级管理员,不可删除修改"
            }).Wait();

            InitUsers(userManager);
            InitMenuTree(dbContext);
        }

        public static void InitUsers(UserManager<SystemUser> userManager)
        {
            for (int i = 1; i <= 9; i++)
            {
                var u = userManager.FindByNameAsync($"admin{i}").Result;
                if (u != null)
                {
                    userManager.DeleteAsync(u).Wait();
                }
                var user = new SystemUser
                {
                    UserName = $"admin{i}",
                    Name = $"管理员{i}",
                    Sex = Sex.Male,
                    Avatar = i.ToString(),
                    PhoneNumber = $"1580000111{i}",
                    IsActive = true,
                    CreatedTime = DateTime.Now
                };
                userManager.CreateAsync(user, $"admin{i}").Wait();
                userManager.AddToRoleAsync(user, "超级管理员").Wait();
            }
        }

        public static void InitMenuTree(SystemIdentityDbContext dbContext)
        {
            dbContext.Set<Menu>().RemoveRange(dbContext.Set<Menu>());
            dbContext.Set<Menu>().Add(new Menu(16, "系统管理", "systemmanage", null, 1, null, 1));
            dbContext.Set<Menu>().Add(new Menu(30, "员工管理", "employeeManage", "allDepartment,employeeList,allPosition", 1, null, 1));
            dbContext.Set<Menu>().Add(new Menu(38, "添加按钮", "addDepartmentBtn", " departmentAdd", 2, null, 1));
            dbContext.Set<Menu>().Add(new Menu(40, "删除按钮", "deleteDepartmentBtn", "departmentDelete", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(41, "查看页面", null, null, 3, null, 0));
            dbContext.Set<Menu>().Add(new Menu(42, "查看页面", null, null, 3, null, 0));
            dbContext.Set<Menu>().Add(new Menu(43, "查看页面", null, null, 3, null, 0));
            dbContext.Set<Menu>().Add(new Menu(44, "查看页面", null, null, 3, null, 0));
            dbContext.Set<Menu>().Add(new Menu(45, "查看页面", null, null, 3, null, 0));
            dbContext.Set<Menu>().Add(new Menu(39, "更新按钮", "updateDepartmentBtn", "departmentGet,departmentUpdate", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(47, "查看页面", null, null, 3, null, 0));
            dbContext.Set<Menu>().Add(new Menu(28, "删除按钮", "deleteMenuBtn", "menuDelete", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(46, "查看页面", null, null, 3, null, 0));
            dbContext.Set<Menu>().Add(new Menu(2, "租户管理", "tenantManage", "tenantList", 1, null, 1));
            dbContext.Set<Menu>().Add(new Menu(26, "添加按钮", "addMenuBtn", "menuAdd", 2, null, 1));
            dbContext.Set<Menu>().Add(new Menu(31, "职位管理", "positionManage", "positionList", 1, null, 2));
            dbContext.Set<Menu>().Add(new Menu(27, "更新按钮", "updateMenuBtn", "menuUpdate", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(5, "删除按钮", "deleteTenantBtn", "tenantDelete", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(4, "更新按钮", "updateTenantBtn", "tenantGet,tenantUpdate", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(25, "删除按钮", "deleteRoleBtn", "roleDelete", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(33, "部门管理", "departmentManage", "allDepartment,userDic", 1, null, 3));
            dbContext.Set<Menu>().Add(new Menu(29, "组织管理", "groupmanage", null, 1, null, 2));
            dbContext.Set<Menu>().Add(new Menu(34, "更新按钮", "updateEmployeeBtn", "employeeGet,employeeUpdate", 2, null, 1));
            dbContext.Set<Menu>().Add(new Menu(70, "字典管理", "dicManage", "dicTypeList,dicDataList", 1, null, 4));
            dbContext.Set<Menu>().Add(new Menu(71, "查看页面", null, null, 3, null, 1));
            dbContext.Set<Menu>().Add(new Menu(73, "更新类型按钮", "updateDicTypeBtn", "dicTypeGet,dicTypeUpdate", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(72, "添加类型按钮", "addDicTypeBtn", "dicTypeAdd", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(74, "删除类型按钮", "deleteDicTypeBtn", "dicTypeDelete", 2, null, 4));
            dbContext.Set<Menu>().Add(new Menu(75, "添加内容按钮", "addDicDataBtn", "dicDataAdd", 2, null, 5));
            dbContext.Set<Menu>().Add(new Menu(76, "更新内容按钮", "updateDicDataBtn", "dicDataGet,dicDataUpdate", 2, null, 6));
            dbContext.Set<Menu>().Add(new Menu(77, "删除内容按钮", "deleteDicDataBtn", "dicDataDelete", 2, null, 7));
            dbContext.Set<Menu>().Add(new Menu(3, "添加按钮", "addTenantBtn", "tenantAdd", 2, null, 1));
            dbContext.Set<Menu>().Add(new Menu(37, "删除按钮", "deletePositionBtn", "positionDelete", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(8, "仪表盘", "dashaboard", null, 1, null, 0));
            dbContext.Set<Menu>().Add(new Menu(35, "添加按钮", "addPositionBtn", "positionAdd", 2, null, 1));
            dbContext.Set<Menu>().Add(new Menu(36, "更新按钮", "updatePositionBtn", "positionGet,positionUpdate", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(19, "菜单管理", "menuManage", "menuList", 1, null, 3));
            dbContext.Set<Menu>().Add(new Menu(18, "角色管理", "roleManage", "roleList,menuList", 1, null, 2));
            dbContext.Set<Menu>().Add(new Menu(24, "更新按钮", "updateRoleBtn", "roleGet,roleUpdate", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(17, "用户管理", "userManage", "userList,roleNameList", 1, null, 1));
            dbContext.Set<Menu>().Add(new Menu(1, "SAAS管理", "saasmanage", null, 1, null, 4));
            dbContext.Set<Menu>().Add(new Menu(53, "添加按钮", "addArticleBtn", "articleAdd", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(49, "文章管理", "articleManage", "allColumn,articleList", 1, null, 1));
            dbContext.Set<Menu>().Add(new Menu(54, "更新按钮", "updateArticleBtn", "articleGet,articleUpdate", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(55, "删除按钮", "deleteArticleBtn", "articleDelete", 2, null, 4));
            dbContext.Set<Menu>().Add(new Menu(50, "栏目管理", "columnManage", "allColumn", 1, null, 2));
            dbContext.Set<Menu>().Add(new Menu(20, "添加按钮", "adduserbtn", "userAdd", 2, null, 1));
            dbContext.Set<Menu>().Add(new Menu(56, "查看页面", null, null, 3, null, 1));
            dbContext.Set<Menu>().Add(new Menu(58, "更新按钮", "updateColumnBtn", "columnGet,columnUpdate", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(52, "查看页面", null, null, 3, null, 1));
            dbContext.Set<Menu>().Add(new Menu(60, "查看页面", null, null, 3, null, 1));
            dbContext.Set<Menu>().Add(new Menu(22, "删除按钮", "deleteUserBtn", "userDelete", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(51, "文件管理", "fileManage", "fileList", 1, null, 3));
            dbContext.Set<Menu>().Add(new Menu(23, "添加按钮", "addRoleBtn", "roleAdd", 2, null, 1));
            dbContext.Set<Menu>().Add(new Menu(59, "删除按钮", "deleteColumnBtn", "columnDelete", 2, null, 4));
            dbContext.Set<Menu>().Add(new Menu(21, "更新按钮", "updateUserBtn", "userDetail,userUpdate", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(63, "删除文件", "deleteFileBtn", "fileDelete", 2, null, 4));
            dbContext.Set<Menu>().Add(new Menu(62, "上传文件", "uploadFileBtn", "fileAdd", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(57, "添加按钮", "addColumnBtn", "columnAdd", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(68, "Hangfire", "hangfire", null, 3, null, 3));
            dbContext.Set<Menu>().Add(new Menu(67, "Swagger", "swagger", null, 3, null, 2));
            dbContext.Set<Menu>().Add(new Menu(66, "代码生成", "code", null, 3, null, 1));
            dbContext.Set<Menu>().Add(new Menu(64, "下载文件", "downloadFileBtn", "fileGet", 2, null, 5));
            dbContext.Set<Menu>().Add(new Menu(61, "创建文件夹", "createFolderBtn", "fileList", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(69, "CAP", "cap", null, 3, null, 4));
            dbContext.Set<Menu>().Add(new Menu(48, "内容管理", "contentmanage", null, 1, null, 5));
            dbContext.Set<Menu>().Add(new Menu(65, "系统工具", "systemtool", null, 1, null, 6));
            dbContext.Set<Menu>().Add(new Menu(81, "工作流管理", "workflowManage", "workflowGet,workflowList,workflowDelete,workflowAdd,workflowUpdate,workflowPublish,workflowFlowGet,workflowFlowAddUpdate,workflowFormGet,workflowFormAddUpdate,workflowFormControlDic,workflowGroupGet,allworkflowGroup,workflowGroupDelete,workflowGroupAdd,workflowGroupUpdate,departmentDic,positionDic", 1, null, 3));
            dbContext.Set<Menu>().Add(new Menu(78, "工作流", "workflow", null, 1, null, 3));
            dbContext.Set<Menu>().Add(new Menu(79, "我创建的", "myWorkflow", "workflowList,workflowFlowGet,workflowFormGet,allworkflowGroup,workFlowInstanceAdd,workFlowInstanceDelete,workFlowInstanceList,workFlowInstanceValuesGet,workFlowInstanceValuesUpdate,workFlowInstanceRouteList,workFlowInstanceSubmit,workFlowInstanceCancel", 1, null, 1));
            dbContext.Set<Menu>().Add(new Menu(80, "我参与的", "handledWorkflow", "workflowList,workflowFlowGet,workflowFormGet,allworkflowGroup,workFlowInstanceValuesGet,workFlowInstanceRouteList,handledWorkFlowInstanceList,handleWorkFlowInstanceApprove", 1, null, 2));

            dbContext.Set<MenuTree>().RemoveRange(dbContext.Set<MenuTree>());
            dbContext.Set<MenuTree>().Add(new MenuTree(95, 33, 43, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(58, 28, 28, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(65, 29, 33, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(94, 29, 43, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(93, 42, 42, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(2, 1, 2, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(10, 1, 5, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(53, 16, 27, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(51, 19, 26, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(1, 1, 1, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(50, 16, 26, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(54, 19, 27, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(55, 27, 27, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(56, 16, 28, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(57, 19, 28, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(48, 18, 25, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(67, 29, 34, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(38, 16, 22, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(52, 26, 26, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(68, 30, 34, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(96, 43, 43, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(70, 29, 35, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(46, 24, 24, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(99, 44, 44, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(98, 17, 44, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(97, 16, 44, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(61, 30, 30, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(63, 31, 31, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(66, 33, 33, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(81, 38, 38, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(69, 34, 34, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(74, 31, 36, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(76, 29, 37, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(80, 33, 38, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(78, 37, 37, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(79, 29, 38, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(73, 29, 36, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(77, 31, 37, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(72, 35, 35, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(62, 29, 31, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(75, 36, 36, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(3, 2, 2, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(175, 48, 74, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(5, 2, 3, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(167, 70, 71, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(168, 71, 71, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(169, 48, 72, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(170, 70, 72, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(171, 72, 72, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(172, 48, 73, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(173, 70, 73, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(174, 73, 73, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(166, 48, 71, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(100, 18, 45, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(177, 74, 74, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(178, 48, 75, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(179, 70, 75, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(180, 75, 75, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(181, 48, 76, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(182, 70, 76, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(183, 76, 76, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(184, 48, 77, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(176, 70, 74, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(4, 1, 3, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(165, 70, 70, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(163, 69, 69, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(6, 3, 3, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(92, 31, 42, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(91, 29, 42, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(90, 41, 41, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(89, 30, 41, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(88, 29, 41, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(87, 40, 40, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(86, 33, 40, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(164, 48, 70, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(44, 16, 24, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(30, 16, 19, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(47, 16, 25, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(11, 2, 5, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(49, 25, 25, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(9, 4, 4, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(8, 2, 4, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(7, 1, 4, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(59, 29, 29, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(162, 65, 69, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(101, 16, 45, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(146, 48, 62, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(103, 19, 46, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(39, 17, 22, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(37, 21, 21, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(36, 17, 21, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(35, 16, 21, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(34, 20, 20, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(33, 17, 20, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(32, 16, 20, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(31, 19, 19, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(40, 22, 22, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(107, 1, 47, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(28, 16, 18, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(27, 17, 17, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(25, 16, 16, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(125, 48, 55, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(154, 64, 64, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(127, 55, 55, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(147, 51, 62, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(148, 62, 62, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(29, 18, 18, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(149, 48, 63, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(41, 16, 23, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(26, 16, 17, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(114, 48, 51, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(111, 49, 49, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(109, 48, 48, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(112, 48, 50, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(113, 50, 50, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(110, 48, 49, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(115, 51, 51, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(121, 53, 53, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(124, 54, 54, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(117, 49, 52, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(119, 48, 53, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(120, 49, 53, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(122, 48, 54, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(108, 47, 47, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(123, 49, 54, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(116, 48, 52, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(18, 8, 8, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(12, 5, 5, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(118, 52, 52, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(60, 29, 30, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(150, 51, 63, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(152, 48, 64, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(137, 48, 59, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(138, 50, 59, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(139, 59, 59, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(140, 48, 60, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(141, 51, 60, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(142, 60, 60, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(144, 51, 61, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(43, 23, 23, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(136, 58, 58, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(84, 39, 39, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(82, 29, 39, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(71, 31, 35, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(83, 33, 39, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(102, 45, 45, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(85, 29, 40, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(106, 2, 47, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(105, 46, 46, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(104, 16, 46, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(45, 18, 24, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(151, 63, 63, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(135, 50, 58, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(133, 57, 57, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(153, 51, 64, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(42, 18, 23, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(155, 65, 65, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(156, 65, 66, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(157, 66, 66, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(158, 65, 67, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(159, 67, 67, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(160, 65, 68, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(134, 48, 58, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(161, 68, 68, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(126, 49, 55, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(145, 61, 61, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(143, 48, 61, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(128, 48, 56, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(129, 50, 56, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(130, 56, 56, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(131, 48, 57, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(132, 50, 57, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(185, 70, 77, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(186, 77, 77, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(187, 78, 78, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(188, 78, 79, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(189, 79, 79, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(190, 78, 80, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(191, 80, 80, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(192, 78, 81, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(193, 81, 81, 0));

            dbContext.SaveChanges();
        }
    }
}
