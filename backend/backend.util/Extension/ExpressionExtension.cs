using System;
using System.Linq;
using System.Linq.Expressions;

namespace Convience.Util.Extension
{
    public static class ExpressionExtension
    {
        public static Expression<Func<T, bool>> TrueExpression<T>() { return t => true; }

        public static Expression<Func<T, bool>> FalseExpression<T>() { return t => false; }

        public static Expression<Func<T, bool>> AndIfHaveValue<T>(this Expression<Func<T, bool>> expression,
            string value, Expression<Func<T, bool>> andExpression)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var invokedExpr = Expression.Invoke(expression, andExpression.Parameters.Cast<Expression>());
                var newExp = Expression.And(andExpression.Body, invokedExpr);
                return Expression.Lambda<Func<T, bool>>(newExp, andExpression.Parameters);
            }
            else
            {
                return expression;
            }
        }
    }
}
