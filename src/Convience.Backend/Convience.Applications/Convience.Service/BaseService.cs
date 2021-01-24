using Convience.Model.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Convience.Service
{
    public interface IBaseService
    {
        public PagingResultModel<T> GetPagingResult<T>(IQueryable<T> query, int page, int size) where T : class;

        public IQueryable<T> GetOrderQuery<T>(IQueryable<T> query, string sort, string order,
            Dictionary<string, Expression<Func<T, object>>> properties);
    }

    public class BaseService : IBaseService
    {

        public PagingResultModel<T> GetPagingResult<T>(IQueryable<T> query, int page, int size) where T : class
        {
            var skip = size * (page - 1);
            return new PagingResultModel<T>()
            {
                Data = query.Skip(skip).Take(size).ToList(),
                Count = query.Count()
            };
        }

        /// <summary>
        /// 针对sort和order添加默认排序
        /// </summary>
        public string JoinString(string originString, string addString)
        {
            if (!string.IsNullOrEmpty(originString))
            {
                return $"{originString},{addString}";
            }
            return addString;
        }

        /// <summary>
        /// 设置数据排序方式
        /// </summary>
        public IQueryable<T> GetOrderQuery<T>(IQueryable<T> query, string sort, string order,
            Dictionary<string, Expression<Func<T, object>>> properties)
        {
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                var isOrderQuaryable = false;
                var sortArray = sort.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var orderArray = order.Split(',', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sortArray.Length; i++)
                {
                    var field = sortArray[i];
                    var orderByExpression = properties[field];

                    // 没有相应的字段退出
                    if (orderByExpression == null)
                    {
                        continue;
                    }
                    var o = orderArray[i];

                    // 添加排序
                    if (isOrderQuaryable)
                    {
                        query = o == "ascend" ?
                            (query as IOrderedQueryable<T>).ThenBy(orderByExpression) :
                            (query as IOrderedQueryable<T>).ThenByDescending(orderByExpression);
                    }
                    else
                    {
                        isOrderQuaryable = true;
                        query = o == "ascend" ?
                            query.OrderBy(orderByExpression) :
                            query.OrderByDescending(orderByExpression);
                    }
                }
            }
            return query;
        }
    }
}
