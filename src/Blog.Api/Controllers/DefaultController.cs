using Microsoft.AspNetCore.Mvc;
using System;

namespace Blog.Api.Controllers
{

    public class DefaultController : ControllerBase
    {
        [HttpGet("/")]
        public string Index()
        {
            return Environment.MachineName;
        }


        [HttpGet("/healthz")]
        public string Healthz()
        {
            return "ok";
        }
    }
}