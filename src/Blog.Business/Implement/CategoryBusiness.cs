using Blog.Infrastructure;
using Blog.Infrastructure.DI;
using Blog.Infrastructure.Model;
using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request.Category;
using Blog.Repository;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    [Injector(typeof(ICategoryBusiness), Lifetime = ServiceLifetime.Scoped)]
    public class CategoryBusiness : BaseBusiness<CategoryInfo>, ICategoryBusiness
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryBusiness(ICategoryRepository repository)
        {
            _categoryRepository = repository;
            BaseRepository = repository;
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        [Caching]
        public async Task<List<CategoryInfo>> GetAllCategoryInfos()
        {
            return await base.QueryAll();
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="searchRequest">查询参数</param>
        /// <returns></returns>
        public async Task<JsonResultModel<CategoryInfo>> GetPageList(CategorySearchRequest searchRequest)
        {
            var exp = Expressionable.Create<CategoryInfo>()
                .OrIF(!string.IsNullOrEmpty(searchRequest.CategoryName),
                    it => it.CategoryName.Contains(searchRequest.CategoryName)).ToExpression();
            return await base.GetPageList(searchRequest, exp);
        }

        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public override async Task<bool> InsertAsync(CategoryInfo entity)
        {
            var any = await _categoryRepository.AnyAsync(it => it.CategoryName == entity.CategoryName);
            if (any)
            {
                throw new ServiceException("category already exist.", "200") { HttpStatusCode = HttpStatusCode.BadRequest };
            }
            return await base.InsertAsync(entity);
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public override async Task<bool> UpdateAsync(CategoryInfo entity)
        {
            var any = await _categoryRepository.AnyAsync(x => x.CategoryName == entity.CategoryName && x.Id != entity.Id);
            if (any)
            {
                throw new ServiceException("category already exist.", "200") { HttpStatusCode = HttpStatusCode.BadRequest };
            }
            return await base.UpdateAsync(entity);
        }
    }
}