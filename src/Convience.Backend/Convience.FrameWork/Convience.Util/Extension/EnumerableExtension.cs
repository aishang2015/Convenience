using System;
using System.Collections.Generic;
using System.Linq;

namespace Convience.Util.Extension
{
    public static class EnumerableExtension
    {

        /// <summary>
        /// 条件与（字符串）
        /// </summary>
        public static IEnumerable<T> AndIfHaveValue<T>(this IEnumerable<T> enumerable,
            string value, Func<T, bool> andFunc)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return enumerable.Where(andFunc);
            }
            return enumerable;
        }

        /// <summary>
        /// 条件与（字符串以外类型）
        /// </summary>
        public static IEnumerable<T> AndIfHaveValue<T>(this IEnumerable<T> enumerable,
            object value, Func<T, bool> andFunc)
        {
            if (value != null)
            {
                return enumerable.Where(andFunc);
            }
            return enumerable;
        }

        /// <summary>
        /// 条件与
        /// </summary>
        public static IEnumerable<T> AndIfCondition<T>(this IEnumerable<T> enumerable,
            bool result, Func<T, bool> andFunc)
        {
            if (result)
            {
                return enumerable.Where(andFunc);
            }
            return enumerable;
        }

        /// <summary>
        /// 与
        /// </summary>
        public static IEnumerable<T> And<T>(this IEnumerable<T> enumerable, Func<T, bool> andFunc)
        {
            return enumerable.Where(andFunc);
        }

        /// <summary>
        /// 或
        /// </summary>
        public static IEnumerable<T> Or<T>(this IEnumerable<T> enumerable, Func<T, bool> andFunc)
        {
            return enumerable.Or(andFunc);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">查询</param>
        /// <param name="page">页码从1开始</param>
        /// <param name="size">每页数据量</param>
        /// <returns></returns>
        public static IEnumerable<T> PageBy<T>(this IEnumerable<T> enumerable, int page, int size)
        {
            return enumerable.Skip((page - 1) * size).Take(size);
        }
    }
}
