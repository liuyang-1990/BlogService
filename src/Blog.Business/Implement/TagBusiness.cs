using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Repository;
using NLog;
using System;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    public class TagBusiness : BaseBusiness<TagInfo>, ITagBusiness
    {
        private readonly ITagRespoitory _tagRespoitory;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public TagBusiness(ITagRespoitory respoitory)
        {
            base.BaseRepository = respoitory;
            _tagRespoitory = respoitory;
        }

        public override async Task<BaseResponse> Insert(TagInfo entity)
        {
            var response = new BaseResponse();
            try
            {
                var isExist = await _tagRespoitory.IsExist(entity);
                if (!isExist) return await base.Insert(entity);
                response.Code = (int)ResponseStatus.AlreadyExists;
                response.Msg = string.Format(MessageConst.AlreadyExists, "tag");
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