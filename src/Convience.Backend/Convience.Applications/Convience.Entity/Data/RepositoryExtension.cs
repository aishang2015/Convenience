using Convience.EntityFrameWork.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Convience.Entity.Data
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IUnitOfWork<SystemIdentityDbContext>, SystemIdentityDbUnitOfWork>();
            services.AddScoped<SystemIdentityDbUnitOfWork, SystemIdentityDbUnitOfWork>();
            return services;
        }
    }
}
