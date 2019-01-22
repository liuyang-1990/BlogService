using Blog.Infrastructure;
using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using NLog;
using SqlSugar;
using System;
using System.Threading.Tasks;
using Blog.Model.Request;

namespace Blog.Business.Implement
{
    public class UserBusiness : BaseBusiness<UserInfo>, IUserBusiness
    {
        private readonly IMd5Helper _md5Helper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public UserBusiness(IUserRepository repository, IMd5Helper md5Helper)
        {
            BaseRepository = repository;
            _md5Helper = md5Helper;
            _userRepository = repository;
        }


        public Task<JsonResultModel<UserInfo>> GetPageList(int pageIndex, int pageSize, UserRequest userInfo)
        {
            var exp = Expressionable.Create<UserInfo>()
                .OrIF(!string.IsNullOrEmpty(userInfo.UserName), it => it.UserName.Contains(userInfo.UserName))
                .AndIF(true, it => it.Status == userInfo.Status).ToExpression();
            return base.GetPageList(pageIndex, pageSize, exp);
        }

        /// <inheritdoc cref="BaseBusiness{T}" />
        public override async Task<BaseResponse> Insert(UserInfo user)
        {
            var response = new BaseResponse();
            try
            {
                var isExist = await _userRepository.IsExist(user);
                if (isExist)
                {
                    response.Code = (int)ResponseStatus.AlreadyExists;
                    response.Msg = string.Format(MessageConst.AlreadyExists, "user");
                    return response;
                }
                user.Password = _md5Helper.Encrypt32(user.Password);
                return await base.Insert(user);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                response.Code = (int)ResponseStatus.Fail;
                response.Msg = ex.Message;
            }
            return response;
        }


        public async Task<UserInfo> GetUserByUserName(string userName, string password)
        {
            try
            {
                password = _md5Helper.Encrypt32(password);
                return await _userRepository.GetUserByUserName(userName, password);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }
    }
}