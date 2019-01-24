using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [EnableCors("LimitRequests")]//支持跨域
    [BlogApiController]
    [Authorize(Policy = "Admin")]
    public class TagController : ControllerBase
    {
        private readonly ITagBusiness _tagBusiness;
        public TagController(ITagBusiness tagBusiness)
        {
            _tagBusiness = tagBusiness;
        }


        [HttpGet("page")]
        public async Task<JsonResultModel<TagInfo>> GetPageList(int pageIndex, int pageSize, string tagName)
        {
            return await _tagBusiness.GetPageList(pageIndex, pageSize, tagName);
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