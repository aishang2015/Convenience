using backend.core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace backend.data.Repositories
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            var libs = DependencyContext.Default.CompileLibraries
                .Where(lib => !lib.Serviceable && lib.Type != "referenceassembly").ToList();
            libs.ForEach(lib =>
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));

                foreach (var type in assembly.GetTypes())
                {
                    var attribute = type.GetCustomAttributes(typeof(EntityAttribute), false).FirstOrDefault();
                    if (attribute != null)
                    {
                        if (typeof(TDbContext) == ((EntityAttribute)attribute).dbContextType)
                        {
                            var interfaceRepository = typeof(IRepository<>).MakeGenericType(type);
                            var baseRepository = typeof(BaseRepository<,>).MakeGenericType(type, typeof(TDbContext)); ;
                            services.AddScoped(interfaceRepository, baseRepository);
                        }
                    }
                }
            });

            services.AddScoped<IUnitOfWork<TDbContext>, UnitOfWork<TDbContext>>();

            return services;
        }
    }
}
