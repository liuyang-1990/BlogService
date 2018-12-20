using Blog.Infrastructure;
using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Repository;
using NLog;
using System;
using System.Threading.Tasks;

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