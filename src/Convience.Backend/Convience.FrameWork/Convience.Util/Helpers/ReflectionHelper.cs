using Microsoft.Extensions.DependencyModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Convience.Util.Helpers
{
    public static class ReflectionHelper
    {
        private static List<CompilationLibrary> _libraryList;

        /// <summary>
        /// 获取程序中所有非引用的lib
        /// </summary>
        public static List<CompilationLibrary> LibraryList
        {
            get
            {
                if (_libraryList == null)
                {
                    _libraryList = DependencyContext.Default.CompileLibraries
                        .Where(lib => !lib.Serviceable && lib.Type != "referenceassembly").ToList();
                }
                return _libraryList;
            }
        }

        private static List<Assembly> _assemblyList;

        /// <summary>
        /// 获取程序中所有非引用的lib的程序集
        /// </summary>
        public static List<Assembly> AssemblyList
        {
            get
            {
                if (_assemblyList == null)
                {
                    _assemblyList = LibraryList.Select(lib => AssemblyLoadContext.Default
                        .LoadFromAssemblyName(new AssemblyName(lib.Name))).ToList();
                }
                return _assemblyList;
            }
        }

        /// <summary>
        /// 在程序集中找到所有指定类型的子类
        /// </summary>
        public static List<Type> GetAllSubClass<T>()
        {
            var result = new List<Type>();
            AssemblyList.ForEach(assembly =>
            {
                result.AddRange(assembly.GetTypes().Where(type => type.BaseType == typeof(T)));
            });
            return result;
        }

        /// <summary>
        /// 根据指定接口名称获取接口和实现接口的类
        /// </summary>
        public static List<(Type, Type)> GetInterfaceAndImplementByName(string interfaceName)
        {
            var result = new List<(Type, Type)>();
            AssemblyList.ForEach(assembly =>
            {
                var interfaceTypes = assembly.GetTypes()
                        .Where(type => type.Name.Contains(interfaceName) && type.IsInterface);
                foreach (var interfaceType in interfaceTypes)
                {
                    var implementType = assembly.GetTypes()
                        .FirstOrDefault(type => type.IsClass && interfaceType.IsAssignableFrom(type));
                    result.Add((interfaceType, implementType));
                }
            });
            return result;
        }

        /// <summary>
        /// 获取对象的属性值
        /// </summary>
        public static object GetPropertyValue(object obj, string property)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }


    }
}
