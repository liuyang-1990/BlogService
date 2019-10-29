using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [BlogApiController]
    [EnableCors("LimitRequests")]//支持跨域
    [Authorize(Policy = "Admin")]
    public class TimeLineController : ControllerBase
    {
        private readonly ITimeLineBusiness _timeLineBusiness;
        private IMapper _mapper;
        public TimeLineController(ITimeLineBusiness timeLineBusiness, IMapper mapper)
        {
            _timeLineBusiness = timeLineBusiness;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IEnumerable<Tbl_Time_Line>> GetAll()
        {
            return await _timeLineBusiness.GetList();
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<Tbl_Time_Line> GetDetailInfo(string id)
        {
            return await _timeLineBusiness.GetDetail(id);
        }

        [HttpPost]
        public async Task<ResultModel<string>> AddTimeLine([FromBody]TimeLineRequest request)
        {
            var entity = _mapper.Map<Tbl_Time_Line>(request);
            return await _timeLineBusiness.Insert(entity);
        }

        [HttpPut]
        public async Task<ResultModel<string>> UpdateTimeLine([FromBody]TimeLineRequest request)
        {
            var entity = _mapper.Map<Tbl_Time_Line>(request);
            return await _timeLineBusiness.Update(entity);
        }

        [HttpDelete("{id}")]
        public async Task<ResultModel<string>> DeleteTimeLine(string id)
        {
            return await _timeLineBusiness.Delete(id);
        }

    }
}