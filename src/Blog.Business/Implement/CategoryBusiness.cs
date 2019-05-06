using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using NLog;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    public class CategoryBusiness : BaseBusiness<CategoryInfo>, ICategoryBusiness
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICategoryRepository _categoryRepository;
        public CategoryBusiness(ICategoryRepository respoitory)
        {
            _categoryRepository = respoitory;
            base.BaseRepository = respoitory;
        }

        public async Task<JsonResultModel<CategoryInfo>> GetPageList(GridParams param, string categoryName)
        {
            var exp = Expressionable.Create<CategoryInfo>()
                .OrIF(!string.IsNullOrEmpty(categoryName),
                    it => it.CategoryName.Contains(categoryName)).ToExpression();
            return await base.GetPageList(param, exp);
        }

        public async Task<ResultModel<string>> Insert(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentNullException(nameof(categoryName));
            }
            var response = new ResultModel<string>();
            var entity = new CategoryInfo()
            {
                CategoryName = categoryName
            };
            var isExist = await _categoryRepository.IsExist(entity, UserAction.Add);
            if (!isExist) return await base.Insert(entity);
            response.IsSuccess = false;
            response.Status = "2";//已经存在
            return response;
        }


        public override async Task<ResultModel<string>> Update(CategoryInfo entity)
        {
            var response = new ResultModel<string>();
            var isExist = await _categoryRepository.IsExist(entity, UserAction.Update);
            if (!isExist) return await base.Update(entity);
            response.IsSuccess = false;
            response.Status = "2";//已经存在
            return response;
        }
    }
}