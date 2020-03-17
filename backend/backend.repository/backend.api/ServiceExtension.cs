using Microsoft.Extensions.DependencyInjection;

namespace backend.repository.backend.api
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRespository>();
            return services;
        }
    }
}
