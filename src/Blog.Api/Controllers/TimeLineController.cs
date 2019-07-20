using System.Threading.Tasks;
using Blog.Business;
using Blog.Model.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("LimitRequests")]//支持跨域
    public class TimeLineController : ControllerBase
    {
        private readonly ITimeLineBusiness _timeLineBusiness;
        public TimeLineController(ITimeLineBusiness timeLineBusiness)
        {
            _timeLineBusiness = timeLineBusiness;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<Tbl_Time_Line> GetDetailInfo(int id)
        {
            return await _timeLineBusiness.GetDetail(id);
        }

    }
}