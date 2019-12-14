using Blog.Infrastructure;
using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Blog.Model.Request.User;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    [Injector(typeof(IUserBusiness), Lifetime = ServiceLifetime.Scoped)]
    public class UserBusiness : BaseBusiness<UserInfo>, IUserBusiness
    {
        private readonly IMd5Helper _md5Helper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserBusiness> _logger;
        public UserBusiness(IUserRepository repository, IMd5Helper md5Helper, ILogger<UserBusiness> logger)
        {
            BaseRepository = repository;
            _md5Helper = md5Helper;
            _userRepository = repository;
            _logger = logger;
        }

        /// <summary>
        /// 分页获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<JsonResultModel<UserInfo>> GetPageList(UserSearchRequest request)
        {
            var exp = Expressionable.Create<UserInfo>()
                .AndIF(true, it => it.Status == request.Status)
                .AndIF(!string.IsNullOrEmpty(request.UserName), it => it.UserName.Contains(request.UserName))
                .ToExpression();
            return await _userRepository.QueryByPage(request, exp);
        }


        public override async Task<ResultModel<string>> Insert(UserInfo user)
        {
            var response = new ResultModel<string>();
            var isExist = await _userRepository.QueryIsExist(x => x.UserName == user.UserName);
            if (isExist)
            {
                response.IsSuccess = false;
                response.Status = "2";//已经存在
                return response;
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                user.Password = "123456"; //默认密码
            }
            user.Password = _md5Helper.Encrypt32(user.Password);
            return await base.Insert(user);
        }
        public override async Task<ResultModel<string>> Update(UserInfo user)
        {
            var response = new ResultModel<string>();
            var isExist = await _userRepository.QueryIsExist(x => x.UserName == user.UserName && x.Id != user.Id);
            if (isExist)
            {
                response.IsSuccess = false;
                response.Status = "2";//已经存在
                return response;
            }
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = _md5Helper.Encrypt32(user.Password);
            }
            return await base.Update(user);
        }


        public async Task<UserInfo> GetUserByUserName(string userName, string password)
        {
            password = _md5Helper.Encrypt32(password);
            return await _userRepository.QueryByWhere(x => x.UserName == userName && x.Password == password);
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
            userInfo.Password = _md5Helper.Encrypt32(request.Password);
            response.IsSuccess = await _userRepository.Update(userInfo, it => it.Password);
            response.Status = response.IsSuccess ? "0" : "1";
            return response;

        }

        public async Task<ResultModel<string>> UpdateStatus(UpdateStatusRequest request)
        {
            var response = new ResultModel<string>();
            response.IsSuccess = await _userRepository.UpdateByIds(request.Ids, it => it.Status == request.Status);
            response.Status = response.IsSuccess ? "0" : "1";
            return response;
        }
    }
}