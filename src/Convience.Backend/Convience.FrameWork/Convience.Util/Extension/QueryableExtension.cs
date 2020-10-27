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
    }
}

