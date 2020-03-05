using Blog.Infrastructure.Cryptography;
using Blog.Infrastructure.DI;
using Blog.Infrastructure.Model;
using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request.User;
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

        public async Task<bool> UpdatePassword(ChangePasswordRequest request)
        {
            var userInfo = await GetUserByUserName(request.UserName, request.OldPassword);
            if (userInfo == null)
            {
                throw new ServiceException("old password is not correct.", "200") { HttpStatusCode = HttpStatusCode.BadRequest };
            }
            userInfo.Password = _md5Helper.Encrypt(request.Password);
            return await _userRepository.UpdateAsync(userInfo.Id, it => new UserInfo { Password = userInfo.Password });
        }

        public async Task<bool> UpdateStatus(UpdateStatusRequest request)
        {
            return await _userRepository.UpdateAsync(request.Ids, it => it.IsActive == request.IsActive);
        }
    }
}