using Convience.EntityFrameWork.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace Convience.Entity.Data
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<SystemRole>, BaseRepository<SystemRole, SystemIdentityDbContext>>();
            services.AddScoped<IRepository<SystemRoleClaim>, BaseRepository<SystemRoleClaim, SystemIdentityDbContext>>();
            services.AddScoped<IRepository<SystemUser>, BaseRepository<SystemUser, SystemIdentityDbContext>>();
            services.AddScoped<IRepository<SystemUserClaim>, BaseRepository<SystemUserClaim, SystemIdentityDbContext>>();
            services.AddScoped<IRepository<SystemUserLogin>, BaseRepository<SystemUserLogin, SystemIdentityDbContext>>();
            services.AddScoped<IRepository<SystemUserRole>, BaseRepository<SystemUserRole, SystemIdentityDbContext>>();
            services.AddScoped<IRepository<SystemUserToken>, BaseRepository<SystemUserToken, SystemIdentityDbContext>>();

            services.AddScoped<IUnitOfWork<SystemIdentityDbContext>, SystemIdentityDbUnitOfWork>();
            services.AddScoped<SystemIdentityDbUnitOfWork, SystemIdentityDbUnitOfWork>();
            return services;
        }
    }
}
