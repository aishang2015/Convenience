using Convience.EntityFrameWork.Infrastructure;
using Convience.Util.Helpers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System.Linq;

namespace Convience.EntityFrameWork.Repositories
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            var assemblyList = ReflectionHelper.AssemblyList;

            foreach (var types in assemblyList.Select(assembly => assembly.GetTypes().ToList()))
            {
                types.ForEach(type =>
                {
                    var attribute = type.GetCustomAttributes(typeof(EntityAttribute), false)
                        .FirstOrDefault();
                    if (attribute != null)
                    {
                        if (typeof(TDbContext) == ((EntityAttribute)attribute).DbContextType)
                        {
                            var interfaceRepository = typeof(IRepository<>).MakeGenericType(type);
                            var baseRepository = typeof(BaseRepository<,>).MakeGenericType(type, typeof(TDbContext)); ;
                            services.AddScoped(interfaceRepository, baseRepository);
                        }
                    }
                });
            }

            services.AddScoped<IUnitOfWork<TDbContext>, UnitOfWork<TDbContext>>();

            return services;
        }
    }
}
