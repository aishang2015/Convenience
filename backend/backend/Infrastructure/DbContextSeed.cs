using Backend.Repository.backend.api.Data;

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
            var user = new SystemUser
            {
                UserName = $"admin",
                Name = "管管",
                Sex = Sex.Male,
                Avatar = "0",
                PhoneNumber = "12312341234",
                IsActive = true
            };
            userManager.CreateAsync(user, "admin").Wait();
        }
    }
}
