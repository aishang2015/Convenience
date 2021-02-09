using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.ManagentApi.Infrastructure.Logs;
using Convience.Model.Models.ContentManage;
using Convience.Service.ContentManage;
using Convience.Util.Extension;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers.ContentManage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        [Permission("articleGet")]
        public async Task<IActionResult> GetById(int id)
        {
            var article = await _articleService.GetByIdAsync(id);
            return Ok(article);
        }

        [HttpGet("list")]
        [Permission("articleList")]
        public IActionResult Get([FromQuery] ArticleQueryModel articleQuery)
        {
            return Ok(_articleService.GetArticles(articleQuery));
        }

        [HttpDelete]
        [Permission("articleDelete")]
        [LogFilter("内容管理", "文章管理", "删除文章")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccess = await _articleService.DeleteArticleAsync(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败!");
            }
            return Ok();
        }

        [HttpPost]
        [Permission("articleAdd")]
        [LogFilter("内容管理", "文章管理", "创建文章")]
        public async Task<IActionResult> Add(ArticleViewModel articleViewModel)
        {
            var isSuccess = await _articleService.AddArticleAsync(articleViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("添加失败!");
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("articleUpdate")]
        [LogFilter("内容管理", "文章管理", "更新文章")]
        public async Task<IActionResult> Update(ArticleViewModel articleViewModel)
        {
            var isSuccess = await _articleService.UpdateArticleAsync(articleViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }
    }
}