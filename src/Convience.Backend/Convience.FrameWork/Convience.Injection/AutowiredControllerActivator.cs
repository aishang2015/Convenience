using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

using System;
using System.Reflection;

namespace Convience.Injection
{
    public class AutowiredControllerActivator : IControllerActivator
    {
        private readonly int MaxDeep = 4;

        /// <summary>
        /// 递归的方式从controller层面逐级装配
        /// </summary>
        public object Create(ControllerContext context)
        {
            // 获取控制器类型
            var controllerType = context.ActionDescriptor.ControllerTypeInfo.AsType();

            // 取得实例
            var controllerInstance = CreateInstance(context.HttpContext.RequestServices, controllerType);

            // 返回实例
            return controllerInstance;
        }

        public void Release(ControllerContext context, object controller)
        {
        }

        /// <summary>
        /// 递归创建字段类型实例内的字段实例
        /// </summary>
        private object CreateInstance(IServiceProvider serviceProvider, Type type, int deep = 1)
        {
            // 超过指定深度，返回null
            if (deep > MaxDeep)
            {
                return null;
            }

            var instance = serviceProvider.GetService(type);

            // 当实例可以从容器中获取时，继续在实例中寻找可以自动注入的对象
            if (instance != null)
            {
                // 在类型中查找
                var autowiredFields = instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                // 循环创建实例
                foreach (var field in autowiredFields)
                {
                    var fieldInstance = CreateInstance(serviceProvider, field.FieldType, deep + 1);

                    // 如果实例可以获得,并且其包含自动装配特性，则将其放入字段中
                    if (fieldInstance != null &&
                        field.GetCustomAttribute<AutowiredAttribute>() != null)
                    {
                        field.SetValue(instance, fieldInstance);
                    }
                }
            }
            return instance;
        }
    }
}
