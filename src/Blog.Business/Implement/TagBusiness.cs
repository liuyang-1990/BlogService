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
    [Injector(typeof(ITagBusiness), LifeTime = Lifetime.Scoped)]
    public class TagBusiness : BaseBusiness<TagInfo>, ITagBusiness
    {
        private readonly ITagRepository _tagRepository;

        public TagBusiness(ITagRepository repository)
        {
            base.BaseRepository = repository;
            _tagRepository = repository;
        }

        public override async Task<ResultModel<string>> Insert(TagInfo entity)
        {
            var response = new ResultModel<string>();
            var isExist = await _tagRepository.IsExist(entity, UserAction.Add);
            if (!isExist)
            {
                return await base.Insert(entity);
            }
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

        [Caching]
        public async Task<List<TagInfo>> GetAllTags()
        {
            return await _tagRepository.GetAllTags();
        }


        public override async Task<ResultModel<string>> Update(TagInfo entity)
        {
            var response = new ResultModel<string>();
            var isExist = await _tagRepository.IsExist(entity, UserAction.Update);
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