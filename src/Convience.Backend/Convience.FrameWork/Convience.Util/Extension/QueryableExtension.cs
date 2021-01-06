using System;
using System.Linq;
using System.Linq.Expressions;

namespace Convience.Util.Extension
{
    public static class QueryableExtension
    {
        /// <summary>
        /// 条件与（字符串）
        /// </summary>
        public static IQueryable<T> AndIfHaveValue<T>(this IQueryable<T> queryable,
            string value, Expression<Func<T, bool>> andExpression)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return queryable.Where(andExpression);
            }
            return queryable;
        }

        /// <summary>
        /// 条件与（字符串以外类型）
        /// </summary>
        public static IQueryable<T> AndIfHaveValue<T>(this IQueryable<T> queryable,
            object value, Expression<Func<T, bool>> andExpression)
        {
            if (value != null)
            {
                return queryable.Where(andExpression);
            }
            return queryable;
        }

        /// <summary>
        /// 条件与
        /// </summary>
        public static IQueryable<T> AndIfCondition<T>(this IQueryable<T> queryable,
            bool result, Expression<Func<T, bool>> andExpression)
        {
            if (result)
            {
                return queryable.Where(andExpression);
            }
            return queryable;
        }

        /// <summary>
        /// 与
        /// </summary>
        public static IQueryable<T> And<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> andExpression)
        {
            return queryable.Where(andExpression);
        }

        /// <summary>
        /// 或
        /// </summary>
        public static IQueryable<T> Or<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> andExpression)
        {
            return queryable.Or(andExpression);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">查询</param>
        /// <param name="page">页码从1开始</param>
        /// <param name="size">每页数据量</param>
        /// <returns></returns>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> queryable, int page, int size)
        {
            return queryable.Skip((page - 1) * size).Take(size);
        }
    }
}

