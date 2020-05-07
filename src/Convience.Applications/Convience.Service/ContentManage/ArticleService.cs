using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.ContentManage;
using Convience.Util.Extension;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public class ArticleService : IArticleService
    {
        private readonly IRepository<Article> _articleRepository;

        private readonly IRepository<Column> _columnRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IMapper _mapper;

        public ArticleService(IRepository<Article> articleRepository,
            IRepository<Column> columnRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _articleRepository = articleRepository;
            _columnRepository = columnRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddArticleAsync(ArticleViewModel model)
        {
            try
            {
                var article = _mapper.Map<Article>(model);
                article.CreateTime = DateTime.Now;
                await _articleRepository.AddAsync(article);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            try
            {
                await _articleRepository.RemoveAsync(id);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public (IEnumerable<ArticleResult>, int) GetArticles(ArticleQuery query)
        {
            Expression<Func<Article, bool>> where = ExpressionExtension.TrueExpression<Article>()
                .AndIfHaveValue(query.Title, a => a.Title.Contains(query.Title))
                .AndIfHaveValue(query.Tag, u => u.Tags.Contains(query.Tag))
                .AndIfHaveValue(query.ColumnId?.ToString(), u => u.ColumnId == query.ColumnId);
            var articleQuery = from article in _articleRepository.Get().Where(@where)
                               join column in _columnRepository.Get() on article.ColumnId equals column.Id into columnInfo
                               from c in columnInfo.DefaultIfEmpty()
                               orderby article.CreateTime descending
                               select new ArticleResult
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
            return (articleQuery.Skip(skip).Take(query.Size), articleQuery.Count());
        }

        public async Task<ArticleResult> GetByIdAsync(int id)
        {
            var article = await _articleRepository.GetAsync(id);
            return _mapper.Map<ArticleResult>(article);
        }

        public async Task<bool> UpdateArticleAsync(ArticleViewModel model)
        {
            try
            {
                var entity = await _articleRepository.GetAsync(model.Id);
                _mapper.Map(model, entity);
                _articleRepository.Update(entity);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
