using Blog.Business;
using Blog.Model.Response;
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
    public class GeographicController : ControllerBase
    {
        private readonly IGeographicBusiness _geographicBusiness;
        public GeographicController(IGeographicBusiness geographicBusiness)
        {
            _geographicBusiness = geographicBusiness;
        }
        [HttpGet("province")]
        public async Task<IEnumerable<Province>> GetProvince()
        {
            return await _geographicBusiness.GetProvince();
        }

        [HttpGet("city/{id}")]
        public async Task<IEnumerable<City>> GetCity(string id)
        {
            return await _geographicBusiness.GetCity(id);
        }
    }
}