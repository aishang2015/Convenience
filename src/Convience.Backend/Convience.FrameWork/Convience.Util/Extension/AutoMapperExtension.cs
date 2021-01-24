using AutoMapper;
using AutoMapper.Configuration;

using Convience.Util.Helpers;

using Microsoft.Extensions.DependencyInjection;

namespace Convience.Util.Extension
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            // 扫描工程中所有Profile文件 加入配置。
            var mapperProfiles = ReflectionHelper.GetAllSubClass<Profile>();

            // 整合profile添加 初始化mapper
            var mapperConfigurationExpression = new MapperConfigurationExpression();
            foreach (var profile in mapperProfiles)
            {
                mapperConfigurationExpression.AddProfile(profile);
            }
            var mapper = new Mapper(new MapperConfiguration(mapperConfigurationExpression));
            services.AddSingleton<IMapper>(mapper);

            return services;
        }
    }
}
