using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using NLog;
using SqlSugar;
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

        public override async Task<ResultModel<string>> Insert(TagInfo entity)
        {
            var response = new ResultModel<string>();
            var isExist = await _tagRespoitory.IsExist(entity, UserAction.Add);
            if (!isExist) return await base.Insert(entity);
            response.IsSuccess = false;
            response.Status = "2";//已经存在
            return response;
        }

        public async Task<JsonResultModel<TagInfo>> GetPageList(GridParams param, string tagName)
        {
            var exp = Expressionable.Create<TagInfo>()
                .OrIF(!string.IsNullOrEmpty(tagName),
                    it => it.TagName.Contains(tagName)).ToExpression();
            return await base.GetPageList(param, exp);
        }


        public override async Task<ResultModel<string>> Update(TagInfo entity)
        {
            var response = new ResultModel<string>();
            var isExist = await _tagRespoitory.IsExist(entity, UserAction.Add);
            if (!isExist) return await base.Update(entity);
            response.IsSuccess = false;
            response.Status = "2";//已经存在
            return response;
        }
    }
}