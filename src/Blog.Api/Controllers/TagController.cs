using Blog.Business;
using Blog.Model;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [BlogApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagBusiness _tagBusiness;
        public TagController(ITagBusiness tagBusiness)
        {
            _tagBusiness = tagBusiness;
        }


        [HttpGet("page")]
        public string GetPageList(int pageIndex, int pageSize)
        {
            return _tagBusiness.GetPageList(pageIndex, pageSize);
        }

        [HttpGet("detail/{id}")]
        public string GetDetailInfo(int id)
        {
            return _tagBusiness.GetDetail(id);
        }

        [HttpPost("add")]
        public bool AddArticle([FromBody]Tag tag)
        {

            return _tagBusiness.Insert(tag);
        }

        [HttpGet("delete")]
        public bool DeleteArticle(int id)
        {
            return _tagBusiness.Delete(id);
        }

        [HttpPost("update")]
        public bool UpdateArticle([FromBody]Tag tag)
        {
            return _tagBusiness.Update(tag);
        }
    }
}