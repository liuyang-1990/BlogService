using Blog.Infrastructure;
using Blog.Infrastructure.DI;
using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Request.Tag;
using Blog.Model.ViewModel;
using Blog.Repository;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    [Injector(typeof(ITagBusiness), Lifetime = ServiceLifetime.Scoped)]
    public class TagBusiness : BaseBusiness<TagInfo>, ITagBusiness
    {
        private readonly ITagRepository _tagRepository;

        public TagBusiness(ITagRepository repository)
        {
            base.BaseRepository = repository;
            _tagRepository = repository;
        }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        [Caching]
        public async Task<List<TagInfo>> GetAllTags()
        {
            return await base.QueryAll();
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="searchRequest">查询参数</param>
        /// <returns></returns>
        public async Task<JsonResultModel<TagInfo>> GetPageList(TagSearchRequest searchRequest)
        {
            var exp = Expressionable.Create<TagInfo>()
                .OrIF(!string.IsNullOrEmpty(searchRequest.TagName),
                    it => it.TagName.Contains(searchRequest.TagName)).ToExpression();
            return await base.GetPageList(searchRequest, exp);
        }

        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public override async Task<bool> InsertAsync(TagInfo entity)
        {
            var any = await _tagRepository.AnyAsync(it => it.TagName == entity.TagName);
            if (any)
            {
                throw new ServiceException("tag already exist.", "200") { HttpStatusCode = HttpStatusCode.BadRequest };
            }
            return await base.InsertAsync(entity);
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public override async Task<bool> UpdateAsync(TagInfo entity)
        {
            var any = await _tagRepository.AnyAsync(it => it.TagName == entity.TagName && it.Id != entity.Id);
            if (any)
            {
                throw new ServiceException("tag already exist.", "200") { HttpStatusCode = HttpStatusCode.BadRequest };
            }
            return await base.UpdateAsync(entity);
        }
    }
}