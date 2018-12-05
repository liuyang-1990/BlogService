using Blog.Business;
using Blog.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    [EnableCors("allowAll")]//支持跨域
    [BlogApiController]
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
        public string GetPageList(int pageIndex, int pageSize)
        {
            return _articleBusiness.GetPageList(pageIndex, pageSize);
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string GetDetailInfo(int id)
        {
            return _articleBusiness.GetDetail(id);
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
        public bool DeleteArticle(int id)
        {
            return _articleBusiness.Delete(id);
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