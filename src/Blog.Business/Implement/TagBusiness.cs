using Blog.Infrastructure;
using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Blog.Model.Request.Tag;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Collections.Generic;
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
        public override async Task<ResultModel<string>> Insert(TagInfo entity)
        {
            var response = new ResultModel<string>();
            var isExist = await _tagRepository.QueryIsExist(it => it.TagName == entity.TagName);
            if (!isExist)
            {
                return await base.Insert(entity);
            }
            response.IsSuccess = false;
            response.Status = "2";//已经存在
            return response;
        }
        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public override async Task<ResultModel<string>> Update(TagInfo entity)
        {
            var response = new ResultModel<string>();
            var isExist = await _tagRepository.QueryIsExist(it => it.TagName == entity.TagName && it.Id != entity.Id);
            if (!isExist)
            {
                return await base.Update(entity);
            }
            response.IsSuccess = false;
            response.Status = "2";//已经存在
            return response;
        }
    }
}