using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    [Injector(typeof(ICategoryBusiness), LifeTime = Lifetime.Scoped)]
    public class CategoryBusiness : BaseBusiness<CategoryInfo>, ICategoryBusiness
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryBusiness(ICategoryRepository repository)
        {
            _categoryRepository = repository;
            base.BaseRepository = repository;
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
        /// <param name="param">查询参数</param>
        /// <param name="categoryName">分类名</param>
        /// <returns></returns>
        public async Task<JsonResultModel<CategoryInfo>> GetPageList(GridParams param, string categoryName)
        {
            var exp = Expressionable.Create<CategoryInfo>()
                .OrIF(!string.IsNullOrEmpty(categoryName),
                    it => it.CategoryName.Contains(categoryName)).ToExpression();
            return await base.GetPageList(param, exp);
        }

        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public override async Task<ResultModel<string>> Insert(CategoryInfo entity)
        {
            var response = new ResultModel<string>();
            var isExist = await _categoryRepository.QueryIsExist(it => it.CategoryName == entity.CategoryName);
            if (!isExist)
            {
                return await base.Insert(entity);
            }
            response.IsSuccess = false;
            response.Status = "2";//已经存在
            return response;
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns></returns>
        public override async Task<ResultModel<string>> Update(CategoryInfo entity)
        {
            var response = new ResultModel<string>();
            var isExist = await _categoryRepository.QueryIsExist(x => x.CategoryName == entity.CategoryName && x.Id != entity.Id);
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