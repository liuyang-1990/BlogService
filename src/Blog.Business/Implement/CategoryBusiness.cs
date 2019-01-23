using Blog.Model;
using Blog.Model.Db;
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

        public async Task<JsonResultModel<CategoryInfo>> GetPageList(int pageIndex, int pageSize, string categoryName)
        {
            var exp = Expressionable.Create<CategoryInfo>()
                .OrIF(!string.IsNullOrEmpty(categoryName),
                    it => it.CategoryName.Contains(categoryName)).ToExpression();
            return await base.GetPageList(pageIndex, pageSize, exp);
        }

        public async Task<BaseResponse> Insert(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentNullException(nameof(categoryName));
            }
            var response = new BaseResponse();
            var entity = new CategoryInfo()
            {
                CategoryName = categoryName
            };
            try
            {
                var isExist = await _categoryRepository.IsExist(entity, UserAction.Add);
                if (!isExist)
                {
                    return await base.Insert(entity);
                }
                response.Code = (int)ResponseStatus.AlreadyExists;
                response.Msg = string.Format(MessageConst.AlreadyExists, "category");
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                response.Code = (int)ResponseStatus.Fail;
                response.Msg = ex.Message;
            }
            return response;
        }


        public override async Task<BaseResponse> Update(CategoryInfo entity)
        {
            var response = new BaseResponse();
            try
            {
                var isExist = await _categoryRepository.IsExist(entity, UserAction.Update);
                if (!isExist)
                {
                    return await base.Update(entity);
                }
                response.Code = (int)ResponseStatus.AlreadyExists;
                response.Msg = string.Format(MessageConst.AlreadyExists, "category");
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                response.Code = (int)ResponseStatus.Fail;
                response.Msg = ex.Message;
            }
            return response;
        }
    }
}