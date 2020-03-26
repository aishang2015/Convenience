
using Backend.Entity.backend.api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace backend.api.Infrastructure
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
            var user = new SystemUser
            {
                UserName = $"admin",
                Name = "管管",
                Sex = Sex.Male,
                Avatar = "1",
                PhoneNumber = "15800001111",
                IsActive = true,
                CreatedTime = DateTime.Now
            };
            userManager.CreateAsync(user, "admin").Wait();
            userManager.AddToRoleAsync(user, "超级管理员").Wait();

            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (16, '系统管理', 'systemmanage', NULL, 1, NULL, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (17, '用户管理', 'userManage', 'userList,roleNameList', 1, NULL, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (18, '角色管理', 'roleManage', 'roleList,menuList	', 1, NULL, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (19, '菜单管理', 'menuManage', 'menuList', 1, NULL, 3)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (20, '添加按钮', 'adduserbtn', 'userAdd', 2, NULL, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (21, '更新按钮', 'updateUserBtn', 'userDetail,userUpdate', 2, NULL, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (22, '删除按钮', 'deleteUserBtn', 'userDelete', 2, NULL, 3)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (23, '添加按钮', 'addRoleBtn', 'roleAdd', 2, NULL, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (24, '更新按钮', 'updateRoleBtn', 'roleGet,roleUpdate	', 2, NULL, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (25, '删除按钮', 'deleteRoleBtn', 'roleDelete', 2, NULL, 3)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (26, '添加按钮', 'addMenuBtn', 'menuAdd', 2, NULL, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (27, '更新按钮', 'updateMenuBtn', 'menuUpdate', 2, NULL, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"Menu\" VALUES (28, '删除按钮', 'deleteMenuBtn', 'menuDelete', 2, NULL, 3)");

            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (25, 16, 16, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (26, 16, 17, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (27, 17, 17, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (28, 16, 18, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (29, 18, 18, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (30, 16, 19, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (31, 19, 19, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (32, 16, 20, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (33, 17, 20, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (34, 20, 20, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (35, 16, 21, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (36, 17, 21, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (37, 21, 21, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (38, 16, 22, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (39, 17, 22, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (40, 22, 22, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (41, 16, 23, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (42, 18, 23, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (43, 23, 23, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (44, 16, 24, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (45, 18, 24, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (46, 24, 24, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (47, 16, 25, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (48, 18, 25, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (49, 25, 25, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (50, 16, 26, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (51, 19, 26, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (52, 26, 26, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (53, 16, 27, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (54, 19, 27, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (55, 27, 27, 0)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (56, 16, 28, 2)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (57, 19, 28, 1)");
            dbContext.Database.ExecuteSqlRaw("INSERT INTO \"MenuTree\" VALUES (58, 28, 28, 0)");

        }
    }
}
