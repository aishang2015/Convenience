using Backend.Repository.backend.api;

using Microsoft.Extensions.DependencyInjection;

namespace backend.repository.backend.api
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            return services;
        }
    }
}
