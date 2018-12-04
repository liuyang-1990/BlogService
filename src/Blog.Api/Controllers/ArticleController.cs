using Blog.Business;
using Blog.Model;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [BlogApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleBusiness _articleBusiness;

        public ArticleController(IArticleBusiness articleBusiness)
        {
            _articleBusiness = articleBusiness;
        }

        [HttpGet("page")]
        public string GetPageList(int pageIndex, int pageSize)
        {
            return _articleBusiness.GetPageList(pageIndex, pageSize);
        }

        [HttpGet("detail/{id}")]
        public string GetDetailInfo(int id)
        {
            return _articleBusiness.GetDetailInfo(id);
        }

        [HttpPost("add")]
        public bool AddArticle([FromBody]ArticleDto articleDto)
        {

            return _articleBusiness.Insert(articleDto);
        }

        [HttpGet("delete")]
        public bool DeleteArticle(int id)
        {
            return _articleBusiness.Delete(id);
        }

        [HttpPost("update")]
        public bool UpdateArticle([FromBody]ArticleDto articleDto)
        {
            return _articleBusiness.Update(articleDto);
        }
    }
}