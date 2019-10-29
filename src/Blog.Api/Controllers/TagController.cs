using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request.Tag;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace Blog.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [BlogApiController]
    [EnableCors("LimitRequests")]//支持跨域
    [Authorize(Policy = "Admin")]
    public class TagController : ControllerBase
    {
        private readonly ITagBusiness _tagBusiness;
        private readonly IMapper _mapper;
        public TagController(ITagBusiness tagBusiness, IMapper mapper)
        {
            _tagBusiness = tagBusiness;
            _mapper = mapper;
        }

        /// <summary>
        /// 分页获取标签信息
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<JsonResultModel<TagInfo>> GetPageList([FromQuery]TagSearchRequest searchRequest)
        {
            return await _tagBusiness.GetPageList(searchRequest);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IEnumerable<TagInfo>> GetAll()
        {
            return await _tagBusiness.GetAllTags();
        }
        /// <summary>
        /// 获取某个标签的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<TagInfo> GetDetailInfo(string id)
        {
            return await _tagBusiness.GetDetail(id);
        }
        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<string>> AddTag([FromBody]CommonTagRequest request)
        {
            return await _tagBusiness.Insert(_mapper.Map<TagInfo>(request));
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultModel<string>> UpdateTag([FromBody]UpdateTagRequest request)
        {
            return await _tagBusiness.Update(_mapper.Map<TagInfo>(request));
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultModel<string>> DeleteTag(string id)
        {
            return await _tagBusiness.Delete(id);
        }

    }
}