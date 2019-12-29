using Blog.Business;
using Blog.Model.Db;
using Blog.Model.Request.TimeLine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nelibur.ObjectMapper;
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
        public TimeLineController(ITimeLineBusiness timeLineBusiness)
        {
            _timeLineBusiness = timeLineBusiness;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IEnumerable<TimeLine>> GetAll()
        {
            return await _timeLineBusiness.GetList();
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<TimeLine> GetDetailInfo(int id)
        {
            return await _timeLineBusiness.SingleAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> AddTimeLine([FromBody]CommonTimeLineRequest request)
        {
            var success = await _timeLineBusiness.InsertAsync(TinyMapper.Map<TimeLine>(request));
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTimeLine([FromBody]UpdateTimeLineRequest request)
        {
            var success = await _timeLineBusiness.UpdateAsync(TinyMapper.Map<TimeLine>(request));
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<object> DeleteTimeLine(int id)
        {
            var success = await _timeLineBusiness.SoftDeleteAsync(id);
            if (success)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}