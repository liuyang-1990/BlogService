using Blog.Infrastructure;
using Blog.Model.Db;
using Blog.Repository;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    public class UserBusiness : BaseBusiness<UserInfo>, IUserBusiness
    {
        private readonly IMd5Helper _md5Helper;

        public UserBusiness(IUserRepository repository, IMd5Helper md5Helper)
        {
            BaseRepository = repository;
            _md5Helper = md5Helper;
        }

        public override Task<bool> Insert(UserInfo entity)
        {
            entity.Password = _md5Helper.Encrypt32(entity.Password);
            return base.Insert(entity);
        }
    }
}