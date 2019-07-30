using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [BlogApiController]
    [EnableCors("LimitRequests")]//支持跨域
    [Authorize(Policy = "Admin")]
    public class ImageController : ControllerBase
    {
        private readonly string[] _pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "ico" };

        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;
        public ImageController(IHostingEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            var files = Request.Form.Files;
            var webRootPath = _environment.WebRootPath;
            if (files == null || !files.Any())
            {
                return null;
            }
            var formFile = files[0];
            var ext = Path.GetExtension(formFile.FileName);
            if (!_pictureFormatArray.Contains(ext.Substring(1).ToLower()))
            {
                return new JsonResult(new
                {
                    Message = "unable to upload!"
                });
            }
            var size = files.Sum(f => f.Length);
            if (size > 15 * 1024 * 1024 * 8)
            {
                return new JsonResult(new
                {
                    Message = "too large to uplpod!"
                });
            }
            var fileName = Guid.NewGuid() + ext;
            var absolutePath = Path.Combine(webRootPath, "images", DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }
            using (var stream = new FileStream(Path.Combine(absolutePath, fileName), FileMode.CreateNew))
            {
                await formFile.CopyToAsync(stream);
            }
            var domain = _configuration.GetSection("Domain").Value;
            var url = "/images/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + fileName;
            return new JsonResult(new
            {
                Url = url,
                WebUrl = domain + url,
                Message = "Success"
            });
        }

    }
}