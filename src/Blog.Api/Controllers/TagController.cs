using Blog.Business;
using Blog.Model.Db;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Model.Response;

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
        public async Task<JsonResultModel<TagInfo>> GetPageList(int pageIndex, int pageSize)
        {
            return await _tagBusiness.GetPageList(pageIndex, pageSize, null);
        }

        [HttpGet("{id}")]
        public async Task<TagInfo> GetDetailInfo(int id)
        {
            return await _tagBusiness.GetDetail(id);
        }

        [HttpPost]
        public async Task<BaseResponse> AddTag([FromBody]TagInfo tag)
        {

            return await _tagBusiness.Insert(tag);
        }

        [HttpDelete("{id}")]
        public async Task<BaseResponse> DeleteTag(int id)
        {
            return await _tagBusiness.Delete(id);
        }

        [HttpPut]
        public async Task<BaseResponse> UpdateTag([FromBody]TagInfo tag)
        {
            return await _tagBusiness.Update(tag);
        }
    }
}