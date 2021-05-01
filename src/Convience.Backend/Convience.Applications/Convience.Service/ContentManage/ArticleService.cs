using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models;
using Convience.Model.Models.ContentManage;
using Convience.Util.Extension;

using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{

    public interface IArticleService
    {
        Task<ArticleResultModel> GetByIdAsync(int id);

        PagingResultModel<ArticleResultModel> GetArticles(ArticleQueryModel query);

        Task<bool> AddArticleAsync(ArticleViewModel model);

        Task<bool> UpdateArticleAsync(ArticleViewModel model);

        Task<bool> DeleteArticleAsync(int id);
    }

    public class ArticleService : IArticleService
    {
        private readonly ILogger<ArticleService> _logger;

        private readonly IRepository<Article> _articleRepository;

        private readonly IRepository<Column> _columnRepository;

        private readonly SystemIdentityDbUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ArticleService(
            ILogger<ArticleService> logger,
            IRepository<Article> articleRepository,
            IRepository<Column> columnRepository,
            SystemIdentityDbUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _articleRepository = articleRepository;
            _columnRepository = columnRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddArticleAsync(ArticleViewModel model)
        {
            var article = _mapper.Map<Article>(model);
            await _articleRepository.AddAsync(article);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            await _articleRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public PagingResultModel<ArticleResultModel> GetArticles(ArticleQueryModel query)
        {
            Expression<Func<Article, bool>> where = ExpressionExtension.TrueExpression<Article>()
                .AndIfHaveValue(query.Title, a => a.Title.Contains(query.Title))
                .AndIfHaveValue(query.Tag, u => u.Tags.Contains(query.Tag))
                .AndIfHaveValue(query.ColumnId?.ToString(), u => u.ColumnId == query.ColumnId);
            var articleQuery = from article in _articleRepository.Get().Where(@where)
                               join column in _columnRepository.Get() on article.ColumnId equals column.Id into columnInfo
                               from c in columnInfo.DefaultIfEmpty()
                               orderby article.CreateTime descending
                               select new ArticleResultModel
                               {
                                   Id = article.Id,
                                   Title = article.Title,
                                   SubTitle = article.SubTitle,
                                   ColumnId = article.ColumnId,
                                   ColumnName = c.Name,
                                   Source = article.Source,
                                   Sort = article.Sort,
                                   Tags = article.Tags,
                                   Content = article.Content,
                                   CreateTime = article.CreateTime
                               };
            var skip = query.Size * (query.Page - 1);
            return new PagingResultModel<ArticleResultModel>
            {
                Data = articleQuery.Skip(skip).Take(query.Size).ToList(),
                Count = articleQuery.Count()
            };
        }

        public async Task<ArticleResultModel> GetByIdAsync(int id)
        {
            var article = await _articleRepository.GetAsync(id);
            return _mapper.Map<ArticleResultModel>(article);
        }

        public async Task<bool> UpdateArticleAsync(ArticleViewModel model)
        {
            var entity = _mapper.Map<Article>(model);
            _articleRepository.UpdateIgnore(entity, a => a.CreateTime);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
