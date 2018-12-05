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

        [HttpGet("{id}")]
        public string GetDetailInfo(int id)
        {
            return _articleBusiness.GetDetail(id);
        }

        [HttpPost]
        public bool AddArticle([FromBody]ArticleDto articleDto)
        {

            return _articleBusiness.Insert(articleDto);
        }

        [HttpDelete("{id}")]
        public bool DeleteArticle(int id)
        {
            return _articleBusiness.Delete(id);
        }

        [HttpPut]
        public bool UpdateArticle([FromBody]ArticleDto articleDto)
        {
            return _articleBusiness.Update(articleDto);
        }
    }
}