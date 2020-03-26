
using Backend.Entity.backend.api.Data;
using Microsoft.AspNetCore.Identity;
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
        }
    }
}
