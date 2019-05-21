using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    [EnableCors("LimitRequests")]//支持跨域
    [BlogApiController]
    [Authorize(Policy = "Admin")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleBusiness _articleBusiness;

        public ArticleController(IArticleBusiness articleBusiness)
        {
            _articleBusiness = articleBusiness;
        }

        /// <summary>
        /// 分页获取文章
        /// </summary>
        /// <param name="param"></param>
        /// <param name="searchParmas"></param>
        /// <returns></returns>
        [HttpGet("page")]
        [AllowAnonymous]
        public async Task<JsonResultModel<ArticleInfo>> GetPageList(GridParams param, ArticleRequest searchParmas)
        {
            return await _articleBusiness.GetPageList(param, searchParmas);
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ArticleDetailResponse> GetDetailInfo(int id)
        {
            return await _articleBusiness.GetArticleDetail(id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="articleDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<string>> AddArticle([FromBody]ArticleDto articleDto)
        {

            return await _articleBusiness.Insert(articleDto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultModel<string>> DeleteArticle(int id)
        {
            return await _articleBusiness.Delete(id);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="articleDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultModel<string>> UpdateArticle([FromBody]ArticleDto articleDto)
        {
            return await _articleBusiness.Update(articleDto);
        }
        /// <summary>
        /// 根据种类获取文章信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]
        public async Task<List<ArticleInfo>> GetArticleByCategory(int categoryId, int pageIndex, int pageSize)
        {
            return await _articleBusiness.GetArticleByCategory(categoryId, pageIndex, pageSize);
        }

        /// <summary>
        /// 根据标签获取文章信息
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("tag/{tagId}")]
        [AllowAnonymous]
        public async Task<List<ArticleInfo>> GetArticleByTag(int tagId, int pageIndex, int pageSize)
        {
            return await _articleBusiness.GetArticleByTag(tagId, pageIndex, pageSize);
        }
    }
}