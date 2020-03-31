using Microsoft.Extensions.DependencyModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Convience.Util.helpers
{
    public static class ReflectionHelper
    {
        private static List<CompilationLibrary> _libraryList;

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


        public static List<Type> GetAllSubClass<T>()
        {
            var result = new List<Type>();
            AssemblyList.ForEach(assembly =>
            {
                result.AddRange(assembly.GetTypes().Where(type => type.BaseType == typeof(T)));
            });
            return result;
        }

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

        public static object GetPropertyValue(object obj, string property)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }


    }
}
