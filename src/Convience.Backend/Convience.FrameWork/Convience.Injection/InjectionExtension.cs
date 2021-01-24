using Convience.Util.Helpers;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Convience.Injection
{
    public static class InjectionExtension
    {
        /// <summary>
        /// 通过在字段上标记autowired特性，实现自动装配功能。
        /// 该类型必须直接或间接被controller调用才可以。
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutowired(this IServiceCollection services)
        {
            // 需要添加来保证所有的控制器放入di中
            // services.AddControllers()[--->].AddControllersAsServices()[<---]
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, AutowiredControllerActivator>());
            return services;
        }

        /// <summary>
        /// 批量注册(类型条件)
        /// </summary>
        public static IServiceCollection AddScopedBatch(
            this IServiceCollection services,
            Func<Type, bool> predicate)
        {
            GetTypePairs(predicate).ForEach(
                result => services.AddScoped(result.Item1, result.Item2));
            return services;
        }

        /// <summary>
        /// 批量注册(类型条件)
        /// </summary>
        public static IServiceCollection AddSingletonBatch(
            this IServiceCollection services,
            Func<Type, bool> predicate)
        {
            GetTypePairs(predicate).ForEach(
                result => services.AddSingleton(result.Item1, result.Item2));
            return services;
        }

        /// <summary>
        /// 批量注册(类型条件)
        /// </summary>
        public static IServiceCollection AddTransientBatch(
            this IServiceCollection services,
            Func<Type, bool> predicate)
        {
            GetTypePairs(predicate).ForEach(
                result => services.AddTransient(result.Item1, result.Item2));
            return services;
        }

        /// <summary>
        /// 批量注册(单元素泛型)
        /// </summary>
        public static IServiceCollection AddScopedBatch(
            this IServiceCollection services,
            Type serviceGenericType,
            Type implementGenericType,
            Func<Type, bool> predicate)
        {
            GetGenericTypePairs(serviceGenericType, implementGenericType, predicate).ForEach(
                result => services.AddScoped(result.Item1, result.Item2));
            return services;
        }

        /// <summary>
        /// 批量注册(单元素泛型)
        /// </summary>
        public static IServiceCollection AddSingletonBatch(
            this IServiceCollection services,
            Type serviceGenericType,
            Type implementGenericType,
            Func<Type, bool> predicate)
        {
            GetGenericTypePairs(serviceGenericType, implementGenericType, predicate).ForEach(
                result => services.AddSingleton(result.Item1, result.Item2));
            return services;
        }

        /// <summary>
        /// 批量注册(单元素泛型)
        /// </summary>
        public static IServiceCollection AddTransientBatch(
            this IServiceCollection services,
            Type serviceGenericType,
            Type implementGenericType,
            Func<Type, bool> predicate)
        {
            GetGenericTypePairs(serviceGenericType, implementGenericType, predicate).ForEach(
                result => services.AddTransient(result.Item1, result.Item2));
            return services;
        }

        private static List<(Type, Type)> GetTypePairs(Func<Type, bool> predicate)
        {
            var result = new List<(Type, Type)>();

            // 聚合
            var allTypes = ReflectionHelper.AssemblyList.SelectMany(assembly => assembly.GetTypes());

            // 根据条件获取所有抽象类型
            var abstractTypes = allTypes.Where(predicate);

            // 根据抽象类型查找实现类型
            foreach (var abstractType in abstractTypes)
            {
                allTypes.Where(type => type.IsClass && abstractType.IsAssignableFrom(type))
                    .ToList().ForEach(implementType => result.Add((abstractType, implementType)));
            }

            return result;
        }

        private static List<(Type, Type)> GetGenericTypePairs(
            Type serviceGenericType,
            Type implementGenericType,
            Func<Type, bool> predicate)
        {
            var result = new List<(Type, Type)>();

            // 聚合
            var allTypes = ReflectionHelper.AssemblyList.SelectMany(assembly => assembly.GetTypes());

            // 根据条件获取所有抽象类型
            var abstractTypes = allTypes.Where(predicate);

            foreach (var abstractType in abstractTypes)
            {
                var serviceType = serviceGenericType.MakeGenericType(abstractType);
                var implementType = implementGenericType.MakeGenericType(abstractType);
                if (implementType != null)
                {
                    result.Add((serviceType, implementType));
                }
            }

            return result;
        }
    }
}
