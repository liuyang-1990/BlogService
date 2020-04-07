using Blog.Infrastructure;
using Blog.Infrastructure.DI;
using Blog.Infrastructure.Model;
using Blog.Model.Db;
using Blog.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    [Injector(typeof(IFriendLinkBusiness), Lifetime = ServiceLifetime.Scoped)]
    public class FriendLinkBusiness : BaseBusiness<FriendLink>, IFriendLinkBusiness
    {
        private readonly IFriendLinkRepository _friendLinkRepository;
        public FriendLinkBusiness(IFriendLinkRepository repository)
        {
            _friendLinkRepository = repository;
            BaseRepository = repository;
        }
        /// <summary>
        /// 获取所有友链
        /// </summary>
        /// <returns></returns>
        [Caching]
        public async Task<List<FriendLink>> QueryAllAsync()
        {
            return await base.QueryAll();
        }

        /// <summary>
        /// 新增友链
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public override async Task<bool> InsertAsync(FriendLink entity)
        {
            var any = await _friendLinkRepository.AnyAsync(it => it.LinkName == entity.LinkName);
            if (any)
            {
                throw new ServiceException("friend link already exist.", "200") { HttpStatusCode = HttpStatusCode.BadRequest };
            }
            return await base.InsertAsync(entity);
        }

        /// <summary>
        /// 更新友链
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public override async Task<bool> UpdateAsync(FriendLink entity)
        {
            var any = await _friendLinkRepository.AnyAsync(x => x.LinkName == entity.LinkName && x.Id != entity.Id);
            if (any)
            {
                throw new ServiceException("friend link  already exist.", "200") { HttpStatusCode = HttpStatusCode.BadRequest };
            }
            return await base.UpdateAsync(entity);
        }
    }
}