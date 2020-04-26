using Convience.Model.Models.ContentManage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public interface IArticleService
    {
        Task<ArticleResult> GetByIdAsync(int id);

        (IEnumerable<ArticleResult>, int) GetArticles(ArticleQuery query);

        Task<bool> AddArticleAsync(ArticleViewModel model);

        Task<bool> UpdateArticleAsync(ArticleViewModel model);

        Task<bool> DeleteArticleAsync(int id);
    }
}
