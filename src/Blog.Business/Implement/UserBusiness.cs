using Blog.Infrastructure.Cryptography;
using Blog.Infrastructure.DI;
using Blog.Model;
using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request.User;
using Blog.Model.Response;
using Blog.Repository;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Net;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    [Injector(typeof(IUserBusiness), Lifetime = ServiceLifetime.Scoped)]
    public class UserBusiness : BaseBusiness<UserInfo>, IUserBusiness
    {
        private readonly IMd5Helper _md5Helper;
        private readonly IUserRepository _userRepository;
        public UserBusiness(IUserRepository repository, IMd5Helper md5Helper)
        {
            BaseRepository = repository;
            _md5Helper = md5Helper;
            _userRepository = repository;
        }

        /// <summary>
        /// 分页获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<JsonResultModel<UserInfo>> GetPageList(UserSearchRequest request)
        {
            var exp = Expressionable.Create<UserInfo>()
                .AndIF(true, it => it.IsActive == request.IsActive)
                .AndIF(!string.IsNullOrEmpty(request.UserName), it => it.UserName.Contains(request.UserName))
                .ToExpression();
            return await _userRepository.Query(request, exp);
        }


        public override async Task<bool> InsertAsync(UserInfo user)
        {
            var any = await _userRepository.AnyAsync(x => x.UserName == user.UserName);
            if (any)
            {
                throw new ServiceException("user already exist.", "200") { HttpStatusCode = HttpStatusCode.BadRequest };
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                user.Password = "Blog.Core"; //默认密码
            }
            user.Password = _md5Helper.Encrypt(user.Password);
            return await base.InsertAsync(user);
        }
        public override async Task<bool> UpdateAsync(UserInfo user)
        {
            var any = await _userRepository.AnyAsync(x => x.UserName == user.UserName && x.Id != user.Id);
            if (any)
            {
                throw new ServiceException("user already exist.", "200") { HttpStatusCode = HttpStatusCode.BadRequest };
            }
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = _md5Helper.Encrypt(user.Password);
            }
            return await base.UpdateAsync(user);
        }


        public async Task<UserInfo> GetUserByUserName(string userName, string password)
        {
            password = _md5Helper.Encrypt(password);
            return await _userRepository.SingleAsync(x => x.UserName == userName && x.Password == password);
        }

        public async Task<ResultModel<string>> UpdatePassword(ChangePasswordRequest request)
        {
            var response = new ResultModel<string>();
            var userInfo = await GetUserByUserName(request.UserName, request.OldPassword);
            if (userInfo == null)
            {
                response.IsSuccess = false;
                response.Status = "2"; //旧密码不正确
                return response;
            }
            userInfo.Password = _md5Helper.Encrypt(request.Password);
            response.IsSuccess = await _userRepository.UpdateAsync(userInfo.Id, it => new UserInfo { Password = userInfo.Password });
            response.Status = response.IsSuccess ? "0" : "1";
            return response;

        }

        public async Task<ResultModel<string>> UpdateStatus(UpdateStatusRequest request)
        {
            var response = new ResultModel<string>();
            response.IsSuccess = await _userRepository.UpdateAsync(request.Ids, it => it.IsActive == request.IsActive);
            response.Status = response.IsSuccess ? "0" : "1";
            return response;
        }
    }
}