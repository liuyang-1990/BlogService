using Blog.Business;
using Blog.Model.Db;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [EnableCors("allowAll")]//支持跨域
    [BlogApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [HttpGet("page")]
        public string GetPageList(int pageIndex, int pageSize)
        {
            return _userBusiness.GetPageList(pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public string GetDetailInfo(int id)
        {
            return _userBusiness.GetDetail(id);
        }

        [HttpPost]
        public bool AddUser([FromBody]UserInfo user)
        {

            return _userBusiness.Insert(user);
        }

        [HttpDelete("{id}")]
        public bool DeleteUser(int id)
        {
            return _userBusiness.Delete(id);
        }

        [HttpPut]
        public bool UpdateUser([FromBody]UserInfo user)
        {
            return _userBusiness.Update(user);
        }
    }
}