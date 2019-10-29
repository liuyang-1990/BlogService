using AutoMapper;
using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request.TimeLine;
using Blog.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private readonly IMapper _mapper;
        public TimeLineController(ITimeLineBusiness timeLineBusiness, IMapper mapper)
        {
            _timeLineBusiness = timeLineBusiness;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IEnumerable<TimeLine>> GetAll()
        {
            return await _timeLineBusiness.GetList();
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<TimeLine> GetDetailInfo(string id)
        {
            return await _timeLineBusiness.GetDetail(id);
        }

        [HttpPost]
        public async Task<ResultModel<string>> AddTimeLine([FromBody]CommonTimeLineRequest request)
        {
            var timeLine = _mapper.Map<TimeLine>(request);
            return await _timeLineBusiness.Insert(timeLine);
        }

        [HttpPut]
        public async Task<ResultModel<string>> UpdateTimeLine([FromBody]UpdateTimeLineRequest request)
        {
            var timeLine = _mapper.Map<TimeLine>(request);
            return await _timeLineBusiness.Update(timeLine);
        }

        [HttpDelete("{id}")]
        public async Task<ResultModel<string>> DeleteTimeLine(string id)
        {
            return await _timeLineBusiness.Delete(id);
        }

    }
}