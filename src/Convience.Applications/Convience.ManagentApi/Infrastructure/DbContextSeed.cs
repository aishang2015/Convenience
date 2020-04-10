using Convience.Entity.Data;
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
            for (int i = 1; i <= 10; i++)
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
            dbContext.Set<Menu>().Add(new Menu(17, "用户管理", "userManage", "userList,roleNameList", 1, null, 1));
            dbContext.Set<Menu>().Add(new Menu(18, "角色管理", "roleManage", "roleList,menuList	", 1, null, 2));
            dbContext.Set<Menu>().Add(new Menu(19, "菜单管理", "menuManage", "menuList", 1, null, 3));
            dbContext.Set<Menu>().Add(new Menu(20, "添加按钮", "adduserbtn", "userAdd", 2, null, 1));
            dbContext.Set<Menu>().Add(new Menu(21, "更新按钮", "updateUserBtn", "userDetail,userUpdate", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(22, "删除按钮", "deleteUserBtn", "userDelete", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(23, "添加按钮", "addRoleBtn", "roleAdd", 2, null, 1));
            dbContext.Set<Menu>().Add(new Menu(24, "更新按钮", "updateRoleBtn", "roleGet,roleUpdate	", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(25, "删除按钮", "deleteRoleBtn", "roleDelete", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(26, "添加按钮", "addMenuBtn", "menuAdd", 2, null, 1));
            dbContext.Set<Menu>().Add(new Menu(27, "更新按钮", "updateMenuBtn", "menuUpdate", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(28, "删除按钮", "deleteMenuBtn", "menuDelete", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(1, "SAAS管理", "saasmanage", null, 1, null, 2));
            dbContext.Set<Menu>().Add(new Menu(2, "租户管理", "tenantManage", "tenantList", 1, null, 1));
            dbContext.Set<Menu>().Add(new Menu(4, "更新按钮", "updateTenantBtn", "tenantGet,tenantUpdate", 2, null, 2));
            dbContext.Set<Menu>().Add(new Menu(3, "添加按钮", "addTenantBtn", "tenantAdd", 2, null, 1));
            dbContext.Set<Menu>().Add(new Menu(5, "删除按钮", "deleteTenantBtn", "tenantDelete", 2, null, 3));
            dbContext.Set<Menu>().Add(new Menu(8, "仪表盘", "dashaboard", null, 1, null, 0));

            //dbContext.Database.ExecuteSqlRaw("TRUNCATE \"Menu\"");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (16, '系统管理', 'systemmanage', NULL, 1, NULL, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (17, '用户管理', 'userManage', 'userList,roleNameList', 1, NULL, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (18, '角色管理', 'roleManage', 'roleList,menuList	', 1, NULL, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (19, '菜单管理', 'menuManage', 'menuList', 1, NULL, 3)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (20, '添加按钮', 'adduserbtn', 'userAdd', 2, NULL, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (21, '更新按钮', 'updateUserBtn', 'userDetail,userUpdate', 2, NULL, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (22, '删除按钮', 'deleteUserBtn', 'userDelete', 2, NULL, 3)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (23, '添加按钮', 'addRoleBtn', 'roleAdd', 2, NULL, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (24, '更新按钮', 'updateRoleBtn', 'roleGet,roleUpdate	', 2, NULL, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (25, '删除按钮', 'deleteRoleBtn', 'roleDelete', 2, NULL, 3)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (26, '添加按钮', 'addMenuBtn', 'menuAdd', 2, NULL, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (27, '更新按钮', 'updateMenuBtn', 'menuUpdate', 2, NULL, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (28, '删除按钮', 'deleteMenuBtn', 'menuDelete', 2, NULL, 3)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (1,  'SAAS管理', 'saasmanage',NULL,1,NULL,2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (2,  '租户管理', 'tenantManage','tenantList',1,NULL,1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (4,  '更新按钮', 'updateTenantBtn','tenantGet,tenantUpdate',2,NULL,2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (3,  '添加按钮', 'addTenantBtn','tenantAdd',2,NULL,1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (5,  '删除按钮', 'deleteTenantBtn','tenantDelete',2,NULL,3)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (8,  '仪表盘',   'dashaboard',NULL,1,NULL,0)");

            dbContext.Set<MenuTree>().RemoveRange(dbContext.Set<MenuTree>());
            dbContext.Set<MenuTree>().Add(new MenuTree(25, 16, 16, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(26, 16, 17, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(27, 17, 17, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(28, 16, 18, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(29, 18, 18, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(30, 16, 19, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(31, 19, 19, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(32, 16, 20, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(33, 17, 20, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(34, 20, 20, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(35, 16, 21, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(36, 17, 21, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(37, 21, 21, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(38, 16, 22, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(39, 17, 22, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(40, 22, 22, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(41, 16, 23, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(42, 18, 23, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(43, 23, 23, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(44, 16, 24, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(45, 18, 24, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(46, 24, 24, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(47, 16, 25, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(48, 18, 25, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(49, 25, 25, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(50, 16, 26, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(51, 19, 26, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(52, 26, 26, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(53, 16, 27, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(54, 19, 27, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(55, 27, 27, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(56, 16, 28, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(57, 19, 28, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(58, 28, 28, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(1, 1, 1, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(2, 1, 2, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(3, 2, 2, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(4, 1, 3, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(5, 2, 3, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(6, 3, 3, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(7, 1, 4, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(8, 2, 4, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(9, 4, 4, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(10, 1, 5, 2));
            dbContext.Set<MenuTree>().Add(new MenuTree(11, 2, 5, 1));
            dbContext.Set<MenuTree>().Add(new MenuTree(12, 5, 5, 0));
            dbContext.Set<MenuTree>().Add(new MenuTree(18, 8, 8, 0));

            dbContext.SaveChanges();

            //dbContext.Database.ExecuteSqlRaw("TRUNCATE \"MenuTree\"");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (25, 16, 16, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (26, 16, 17, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (27, 17, 17, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (28, 16, 18, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (29, 18, 18, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (30, 16, 19, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (31, 19, 19, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (32, 16, 20, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (33, 17, 20, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (34, 20, 20, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (35, 16, 21, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (36, 17, 21, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (37, 21, 21, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (38, 16, 22, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (39, 17, 22, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (40, 22, 22, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (41, 16, 23, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (42, 18, 23, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (43, 23, 23, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (44, 16, 24, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (45, 18, 24, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (46, 24, 24, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (47, 16, 25, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (48, 18, 25, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (49, 25, 25, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (50, 16, 26, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (51, 19, 26, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (52, 26, 26, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (53, 16, 27, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (54, 19, 27, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (55, 27, 27, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (56, 16, 28, 2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (57, 19, 28, 1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (58, 28, 28, 0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (1,1,1,0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (2,1,2,1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (3,2,2,0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (4,1,3,2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (5,2,3,1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (6,3,3,0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (7,1,4,2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (8,2,4,1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (9,4,4,0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (10,1,5,2)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (11,2,5,1)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (12,5,5,0)");
            //dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (18,8,8,0)");
        }
    }
}
