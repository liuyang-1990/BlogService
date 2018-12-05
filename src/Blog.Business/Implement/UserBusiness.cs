using Blog.Model;
using Blog.Repository;

namespace Blog.Business.Implement
{
    public class UserBusiness : BaseBusiness<User>, IUserBusiness
    {
        public UserBusiness(IUserRepository repository)
        {
            BaseRepository = repository;
        }

    }
}