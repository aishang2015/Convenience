using System;
using System.Linq;
using System.Linq.Expressions;

namespace Convience.Util.Extension
{
    public static class ExpressionExtension
    {
        #region 1

        public static Expression<Func<T, bool>> TrueExpression<T>() { return t => true; }

        public static Expression<Func<T, bool>> FalseExpression<T>() { return t => false; }

        /// <summary>
        /// 条件与（字符串）
        /// </summary>
        public static Expression<Func<T, bool>> AndIfHaveValue<T>(this Expression<Func<T, bool>> expression,
            string value, Expression<Func<T, bool>> andExpression)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 条件与（字符串以外类型）
        /// </summary>
        public static Expression<Func<T, bool>> AndIfHaveValue<T>(this Expression<Func<T, bool>> expression,
            object value, Expression<Func<T, bool>> andExpression)
        {
            if (value != null)
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 与
        /// </summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression, Expression<Func<T, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.And(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T, bool>>(newExp, expression.Parameters);
        }


        /// <summary>
        /// 与
        /// </summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression, Expression<Func<T, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.Or(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T, bool>>(newExp, expression.Parameters);
        }

        #endregion

        #region 2

        public static Expression<Func<T1, T2, bool>> TrueExpression<T1, T2>() { return (t1, t2) => true; }

        public static Expression<Func<T1, T2, bool>> FalseExpression<T1, T2>() { return (t1, t2) => false; }

        /// <summary>
        /// 条件与（字符串）
        /// </summary>
        public static Expression<Func<T1, T2, bool>> AndIfHaveValue<T1, T2>(this Expression<Func<T1, T2, bool>> expression,
            string value, Expression<Func<T1, T2, bool>> andExpression)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 条件与（字符串以外类型）
        /// </summary>
        public static Expression<Func<T1, T2, bool>> AndIfHaveValue<T1, T2>(this Expression<Func<T1, T2, bool>> expression,
            object value, Expression<Func<T1, T2, bool>> andExpression)
        {
            if (value != null)
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 与
        /// </summary>
        public static Expression<Func<T1, T2, bool>> And<T1, T2>(this Expression<Func<T1, T2, bool>> expression, Expression<Func<T1, T2, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.And(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, bool>>(newExp, expression.Parameters);
        }


        /// <summary>
        /// 或
        /// </summary>
        public static Expression<Func<T1, T2, bool>> Or<T1, T2>(this Expression<Func<T1, T2, bool>> expression, Expression<Func<T1, T2, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.Or(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, bool>>(newExp, expression.Parameters);
        }

        #endregion

        #region 3

        public static Expression<Func<T1, T2, T3, bool>> TrueExpression<T1, T2, T3>() { return (t1, t2, t3) => true; }

        public static Expression<Func<T1, T2, T3, bool>> FalseExpression<T1, T2, T3>() { return (t1, t2, t3) => false; }

        /// <summary>
        /// 条件与（字符串）
        /// </summary>
        public static Expression<Func<T1, T2, T3, bool>> AndIfHaveValue<T1, T2, T3>(this Expression<Func<T1, T2, T3, bool>> expression,
            string value, Expression<Func<T1, T2, T3, bool>> andExpression)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 条件与（字符串以外类型）
        /// </summary>
        public static Expression<Func<T1, T2, T3, bool>> AndIfHaveValue<T1, T2, T3>(this Expression<Func<T1, T2, T3, bool>> expression,
            object value, Expression<Func<T1, T2, T3, bool>> andExpression)
        {
            if (value != null)
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 与
        /// </summary>
        public static Expression<Func<T1, T2, T3, bool>> And<T1, T2, T3>(this Expression<Func<T1, T2, T3, bool>> expression, Expression<Func<T1, T2, T3, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.And(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, bool>>(newExp, expression.Parameters);
        }


        /// <summary>
        /// 或
        /// </summary>
        public static Expression<Func<T1, T2, T3, bool>> Or<T1, T2, T3>(this Expression<Func<T1, T2, T3, bool>> expression, Expression<Func<T1, T2, T3, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.Or(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, bool>>(newExp, expression.Parameters);
        }

        #endregion

        #region 4

        public static Expression<Func<T1, T2, T3, T4, bool>> TrueExpression<T1, T2, T3, T4>() { return (t1, t2, t3, t4) => true; }

        public static Expression<Func<T1, T2, T3, T4, bool>> FalseExpression<T1, T2, T3, T4>() { return (t1, t2, t3, t4) => false; }

        /// <summary>
        /// 条件与（字符串）
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, bool>> AndIfHaveValue<T1, T2, T3, T4>(this Expression<Func<T1, T2, T3, T4, bool>> expression,
            string value, Expression<Func<T1, T2, T3, T4, bool>> andExpression)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, T4, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 条件与（字符串以外类型）
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, bool>> AndIfHaveValue<T1, T2, T3, T4>(this Expression<Func<T1, T2, T3, T4, bool>> expression,
            object value, Expression<Func<T1, T2, T3, T4, bool>> andExpression)
        {
            if (value != null)
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, T4, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 与
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, bool>> And<T1, T2, T3, T4>(this Expression<Func<T1, T2, T3, T4, bool>> expression, Expression<Func<T1, T2, T3, T4, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.And(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, T4, bool>>(newExp, expression.Parameters);
        }


        /// <summary>
        /// 或
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, bool>> Or<T1, T2, T3, T4>(this Expression<Func<T1, T2, T3, T4, bool>> expression, Expression<Func<T1, T2, T3, T4, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.Or(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, T4, bool>>(newExp, expression.Parameters);
        }

        #endregion

        #region 5

        public static Expression<Func<T1, T2, T3, T4, T5, bool>> TrueExpression<T1, T2, T3, T4, T5>() { return (t1, t2, t3, t4, t5) => true; }

        public static Expression<Func<T1, T2, T3, T4, T5, bool>> FalseExpression<T1, T2, T3, T4, T5>() { return (t1, t2, t3, t4, t5) => false; }

        /// <summary>
        /// 条件与（字符串）
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, bool>> AndIfHaveValue<T1, T2, T3, T4, T5>(this Expression<Func<T1, T2, T3, T4, T5, bool>> expression,
            string value, Expression<Func<T1, T2, T3, T4, T5, bool>> andExpression)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 条件与（字符串以外类型）
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, bool>> AndIfHaveValue<T1, T2, T3, T4, T5>(this Expression<Func<T1, T2, T3, T4, T5, bool>> expression,
            object value, Expression<Func<T1, T2, T3, T4, T5, bool>> andExpression)
        {
            if (value != null)
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 与
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, bool>> And<T1, T2, T3, T4, T5>(this Expression<Func<T1, T2, T3, T4, T5, bool>> expression, Expression<Func<T1, T2, T3, T4, T5, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.And(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, T4, T5, bool>>(newExp, expression.Parameters);
        }


        /// <summary>
        /// 或
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, bool>> Or<T1, T2, T3, T4, T5>(this Expression<Func<T1, T2, T3, T4, T5, bool>> expression, Expression<Func<T1, T2, T3, T4, T5, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.Or(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, T4, T5, bool>>(newExp, expression.Parameters);
        }

        #endregion

        #region 6

        public static Expression<Func<T1, T2, T3, T4, T5, T6, bool>> TrueExpression<T1, T2, T3, T4, T5, T6>() { return (t1, t2, t3, t4, t5, t6) => true; }

        public static Expression<Func<T1, T2, T3, T4, T5, T6, bool>> FalseExpression<T1, T2, T3, T4, T5, T6>() { return (t1, t2, t3, t4, t5, t6) => false; }

        /// <summary>
        /// 条件与（字符串）
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, bool>> AndIfHaveValue<T1, T2, T3, T4, T5, T6>(this Expression<Func<T1, T2, T3, T4, T5, T6, bool>> expression,
            string value, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> andExpression)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 条件与（字符串以外类型）
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, bool>> AndIfHaveValue<T1, T2, T3, T4, T5, T6>(this Expression<Func<T1, T2, T3, T4, T5, T6, bool>> expression,
            object value, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> andExpression)
        {
            if (value != null)
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 与
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, bool>> And<T1, T2, T3, T4, T5, T6>(this Expression<Func<T1, T2, T3, T4, T5, T6, bool>> expression, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.And(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, bool>>(newExp, expression.Parameters);
        }


        /// <summary>
        /// 或
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, bool>> Or<T1, T2, T3, T4, T5, T6>(this Expression<Func<T1, T2, T3, T4, T5, T6, bool>> expression, Expression<Func<T1, T2, T3, T4, T5, T6, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.Or(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, bool>>(newExp, expression.Parameters);
        }

        #endregion

        #region 7

        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> TrueExpression<T1, T2, T3, T4, T5, T6, T7>() { return (t1, t2, t3, t4, t5, t6, t7) => true; }

        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> FalseExpression<T1, T2, T3, T4, T5, T6, T7>() { return (t1, t2, t3, t4, t5, t6, t7) => false; }

        /// <summary>
        /// 条件与（字符串）
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> AndIfHaveValue<T1, T2, T3, T4, T5, T6, T7>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> expression,
            string value, Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> andExpression)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 条件与（字符串以外类型）
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> AndIfHaveValue<T1, T2, T3, T4, T5, T6, T7>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> expression,
            object value, Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> andExpression)
        {
            if (value != null)
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 与
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> And<T1, T2, T3, T4, T5, T6, T7>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> expression, Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.And(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, bool>>(newExp, expression.Parameters);
        }


        /// <summary>
        /// 或
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> Or<T1, T2, T3, T4, T5, T6, T7>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> expression, Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.Or(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, bool>>(newExp, expression.Parameters);
        }

        #endregion

        #region 8

        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> TrueExpression<T1, T2, T3, T4, T5, T6, T7, T8>() { return (t1, t2, t3, t4, t5, t6, t7, t8) => true; }

        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> FalseExpression<T1, T2, T3, T4, T5, T6, T7, T8>() { return (t1, t2, t3, t4, t5, t6, t7, t8) => false; }

        /// <summary>
        /// 条件与（字符串）
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> AndIfHaveValue<T1, T2, T3, T4, T5, T6, T7, T8>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression,
            string value, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> andExpression)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 条件与（字符串以外类型）
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> AndIfHaveValue<T1, T2, T3, T4, T5, T6, T7, T8>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression,
            object value, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> andExpression)
        {
            if (value != null)
            {
                var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
                var newExp = Expression.And(expression.Body, invokedExpr);
                expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>>(newExp, expression.Parameters);
            }
            return expression;
        }

        /// <summary>
        /// 与
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> And<T1, T2, T3, T4, T5, T6, T7, T8>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.And(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>>(newExp, expression.Parameters);
        }


        /// <summary>
        /// 或
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> Or<T1, T2, T3, T4, T5, T6, T7, T8>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>> andExpression)
        {
            var invokedExpr = Expression.Invoke(andExpression, expression.Parameters.Cast<Expression>());
            var newExp = Expression.Or(expression.Body, invokedExpr);
            return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>>(newExp, expression.Parameters);
        }

        #endregion
    }
}
