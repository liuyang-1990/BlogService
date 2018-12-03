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

        [HttpPost("addArticle")]
        public bool AddArticle([FromBody]ArticleDto articleDto)
        {

            return _articleBusiness.Insert(articleDto);
        }

        [HttpGet("deleteArticle")]
        public bool DeleteArticle(int id)
        {
            return _articleBusiness.Delete(id);
        }

        [HttpPost("updateArticle")]
        public bool UpdateArticle([FromBody]ArticleDto articleDto)
        {
            return _articleBusiness.Update(articleDto);
        }
    }
}