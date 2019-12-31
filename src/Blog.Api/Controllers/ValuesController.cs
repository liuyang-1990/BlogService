using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Blog.Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiVersion("1.1", Deprecated = true)]
    [BlogApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new List<string>() { "v1", "v2" };
        }
    }
}