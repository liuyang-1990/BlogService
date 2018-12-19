using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Repository;
using NLog;
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

        public override async Task<BaseResponse> Insert(CategoryInfo entity)
        {
            var response = new BaseResponse();
            try
            {
                var isExist = await _categoryRepository.IsExist(entity);
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
                response.Code = (int) ResponseStatus.Fail;
                response.Msg = ex.Message;
            }
            return response;
        }
    }
}