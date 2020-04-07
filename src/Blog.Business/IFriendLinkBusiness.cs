using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Model.Db;

namespace Blog.Business
{
    public interface IFriendLinkBusiness : IBaseBusiness<FriendLink>
    {
        /// <summary>
        ///  获取所有友链
        /// </summary>
        /// <returns></returns>
        Task<List<FriendLink>> QueryAllAsync();
    }
}