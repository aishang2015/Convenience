using Convience.Util.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Convience.Util.Extension
{
    public static class ServiceExtension
    {
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

            ReflectionHelper.AssemblyList.ForEach(assembly =>
            {
                var serviceTypes = assembly.GetTypes().Where(predicate);
                foreach (var serviceType in serviceTypes)
                {
                    var implementType = assembly.GetTypes()
                        .FirstOrDefault(type => type.IsClass && serviceType.IsAssignableFrom(type));
                    result.Add((serviceType, implementType));
                }
            });

            return result;
        }

        private static List<(Type, Type)> GetGenericTypePairs(
            Type serviceGenericType,
            Type implementGenericType,
            Func<Type, bool> predicate)
        {
            var result = new List<(Type, Type)>();

            ReflectionHelper.AssemblyList.ForEach(assembly =>
            {
                var baseTypes = assembly.GetTypes().Where(predicate);
                foreach (var type in baseTypes)
                {
                    var serviceType = serviceGenericType.MakeGenericType(type);
                    var implementType = implementGenericType.MakeGenericType(type);
                    result.Add((serviceType, implementType));
                }
            });

            return result;
        }
    }
}
