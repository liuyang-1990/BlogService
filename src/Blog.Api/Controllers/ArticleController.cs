using AutoMapper;
using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Request.Article;
using Blog.Model.Response;
using Blog.Model.ViewModel;
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
    [ApiVersion("1.0")]
    [BlogApiController]
    [EnableCors("LimitRequests")]//支持跨域
    [Authorize(Policy = "Admin")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleBusiness _articleBusiness;
        private readonly IMapper _mapper;

        public ArticleController(IArticleBusiness articleBusiness, IMapper mapper)
        {
            _articleBusiness = articleBusiness;
            _mapper = mapper;
        }

        /// <summary>
        /// 分页获取文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("page")]
        [AllowAnonymous]
        public async Task<JsonResultModel<ArticleInfo>> GetPageList([FromQuery]ArticleSearchRequest request)
        {
            return await _articleBusiness.GetPageList(request);
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ArticleDetailResponse> GetDetailInfo(string id)
        {
            return await _articleBusiness.GetArticleDetail(id);
        }

        /// <summary>
        ///  新增文章
        /// </summary>
        /// <param name="request">文章信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<string>> AddArticle([FromBody]AddArticleRequest request)
        {
            return await _articleBusiness.Insert(request);
        }

        /// <summary>
        ///  更新文章
        /// </summary>
        /// <param name="request">文章信息</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultModel<string>> UpdateArticle([FromBody]UpdateArticleRequest request)
        {
            return await _articleBusiness.Update(request);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultModel<string>> DeleteArticle(string id)
        {
            return await _articleBusiness.Delete(id);
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
            return await _articleBusiness.GetArticleByCategory(categoryId, param);
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
            return await _articleBusiness.GetArticleByTag(tagId, param);
        }
    }
}