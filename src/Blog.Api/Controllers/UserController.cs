using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Business;
using Blog.Model;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [BlogApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        //[HttpGet]
        //public async Task<IEnumerable<User>> GetUsers(User user)
        //{
        //    return await _userBusiness.GetEntities(_ => true);
        //}

        //[HttpPost]
        //public async Task<bool> Add([FromBody]User user)
        //{
        //    return await _userBusiness.Add(user);
        //}

    }
}