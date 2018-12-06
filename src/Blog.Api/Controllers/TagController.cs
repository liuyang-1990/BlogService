using Blog.Business;
using Blog.Model.Db;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [EnableCors("allowAll")]//支持跨域
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

        [HttpGet("{id}")]
        public string GetDetailInfo(int id)
        {
            return _tagBusiness.GetDetail(id);
        }

        [HttpPost]
        public bool AddTag([FromBody]TagInfo tag)
        {

            return _tagBusiness.Insert(tag);
        }

        [HttpDelete("{id}")]
        public bool DeleteTag(int id)
        {
            return _tagBusiness.Delete(id);
        }

        [HttpPut]
        public bool UpdateTag([FromBody]TagInfo tag)
        {
            return _tagBusiness.Update(tag);
        }
    }
}