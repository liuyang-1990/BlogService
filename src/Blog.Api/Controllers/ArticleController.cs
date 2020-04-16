using Blog.Business;
using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Request.Article;
using Blog.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    [ApiController]
    [Route("article")]
    [EnableCors("LimitRequests")]//支持跨域
    [Authorize(Policy = "Admin")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleBusiness _articleBusiness;

        public ArticleController(IArticleBusiness articleBusiness)
        {
            _articleBusiness = articleBusiness;
        }

        [HttpGet("page")]
        [AllowAnonymous]
        public async Task<JsonResultModel<ArticleInfo>> GetPageList([FromQuery]GridParams request)
        {
            return await _articleBusiness.GetPageLsit(request);
        }


        /// <summary>
        /// 分页获取文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("admin/page")]
        public async Task<JsonResultModel<ArticleInfo>> GetPageList([FromQuery]ArticleSearchRequest request)
        {
            return await _articleBusiness.GetPageList(request);
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("admin/{id}")]
        public async Task<ArticleDetailResponse> GetDetailInfo(string id)
        {
            int.TryParse(id, out var aid);
            return await _articleBusiness.GetArticleDetail(aid);
        }


        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ArticleDetailResponse> GetArticleDetailForUser(string id)
        {
            int.TryParse(id, out var aid);
            return await _articleBusiness.GetArticleDetailForUser(aid);
        }

        /// <summary>
        ///  新增文章
        /// </summary>
        /// <param name="request">文章信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddArticle([FromBody]AddArticleRequest request)
        {
            var res = await _articleBusiness.InsertAsync(request);
            if (res.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(res.ResultInfo);
        }

        /// <summary>
        ///  更新文章
        /// </summary>
        /// <param name="request">文章信息</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateArticle([FromBody]UpdateArticleRequest request)
        {
            var res = await _articleBusiness.UpdateAsync(request);
            if (res.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(res.ResultInfo);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(string id)
        {
            int.TryParse(id, out var aid);
            var success = await _articleBusiness.SoftDeleteAsync(aid);
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }


        /// <summary>
        /// 根据种类获取文章信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]
        public async Task<JsonResultModel<ArticleInfo>> GetArticleByCategory(string categoryId, [FromQuery]GridParams param)
        {
            int.TryParse(categoryId, out var cid);
            return await _articleBusiness.GetArticleByCategory(cid, param);
        }

        /// <summary>
        /// 根据标签获取文章信息
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("tag/{tagId}")]
        [AllowAnonymous]
        public async Task<JsonResultModel<ArticleInfo>> GetArticleByTag(string tagId, [FromQuery]GridParams param)
        {
            int.TryParse(tagId, out var tid);
            return await _articleBusiness.GetArticleByTag(tid, param);
        }
    }
}