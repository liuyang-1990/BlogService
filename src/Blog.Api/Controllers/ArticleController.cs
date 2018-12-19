using Blog.Business;
using Blog.Model.Db;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Model.Response;

namespace Blog.Api.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    [EnableCors("allowAll")]//支持跨域
    [BlogApiController]
    [Authorize(Policy = "admin")]
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
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<JsonResultModel<ArticleInfo>> GetPageList(int pageIndex, int pageSize)
        {
            return await _articleBusiness.GetPageList(pageIndex, pageSize, null);
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ArticleInfo> GetDetailInfo(int id)
        {
            return await _articleBusiness.GetDetail(id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="articleDto"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddArticle([FromBody]ArticleDto articleDto)
        {

            return _articleBusiness.Insert(articleDto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<BaseResponse> DeleteArticle(int id)
        {
            return await _articleBusiness.Delete(id);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="articleDto"></param>
        /// <returns></returns>
        [HttpPut]
        public bool UpdateArticle([FromBody]ArticleDto articleDto)
        {
            return _articleBusiness.Update(articleDto);
        }
    }
}