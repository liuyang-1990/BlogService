using Blog.Infrastructure;
using Blog.Model.Db;
using Blog.Repository;

namespace Blog.Business.Implement
{
    public class UserBusiness : BaseBusiness<User>, IUserBusiness
    {
        private readonly IMd5Helper _md5Helper;

        public UserBusiness(IUserRepository repository, IMd5Helper md5Helper)
        {
            BaseRepository = repository;
            _md5Helper = md5Helper;
        }

        public override bool Insert(User entity)
        {
            entity.Password = _md5Helper.Encrypt32(entity.Password);
            return base.Insert(entity);
        }
    }
}