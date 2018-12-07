using Blog.Model.Db;
using Blog.Model.Settings;
using Blog.Model.ViewModel;
using Microsoft.Extensions.Options;
using NLog;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    public class ArticleRepository : BaseRepository<ArticleInfo>, IArticleRepository
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ArticleRepository(IOptions<DbSetting> settings) : base(settings)
        {

        }

        public override Task<ArticleInfo> GetDetail(int id)
        {
            return base.GetDetail(id);
        }

        //public override string GetDetail(int id)
        //{
        //    var info = Context.Db.Queryable<ArticleInfo, ArticleContent>((a, ac) => a.Id == ac.ArticleId)
        //        .Where((a, ac) => a.Id == id)
        //        .Select((a, ac) => new ArticleDto()
        //        {
        //            Id = SqlFunc.GetSelfAndAutoFill(a.Id),
        //            Content = ac.Content
        //        }).Single();
        //    return Context.Db.Utilities.SerializeObject(info);

        //}


        public bool Insert(ArticleInfo article, ArticleContent content)
        {
            try
            {
                Context.Db.Ado.BeginTran();
                var id = Context.Db.Insertable(article).ExecuteReturnIdentity();
                content.ArticleId = id;
                Context.Db.Insertable(content).ExecuteCommand();
                Context.Db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                Context.Db.Ado.RollbackTran();
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 假删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<bool> Delete(int id)
        {

            try
            {
                Context.Db.Ado.BeginTran();
                await Context.Db.Updateable<ArticleInfo>().UpdateColumns(it => it.IsDeleted == 1).Where(it => it.Id == id).ExecuteCommandAsync();
                await Context.Db.Updateable<ArticleContent>().UpdateColumns(it => it.IsDeleted == 1).WhereColumns(it => it.ArticleId == id).ExecuteCommandAsync();
                await Context.Db.Updateable<ArticleImage>().UpdateColumns(it => it.IsDeleted == 1).WhereColumns(it => it.ArticleId == id).ExecuteCommandAsync();
                await Context.Db.Updateable<ArticleCategory>().UpdateColumns(it => it.IsDeleted == 1).WhereColumns(it => it.ArticleId == id).ExecuteCommandAsync();
                await Context.Db.Updateable<ArticleTag>().UpdateColumns(it => it.IsDeleted == 1).WhereColumns(it => it.ArticleId == id).ExecuteCommandAsync();
                Context.Db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                Context.Db.Ado.RollbackTran();
                return false;
            }

        }

        public bool Update(ArticleInfo article, ArticleContent content)
        {
            try
            {
                Context.Db.Ado.BeginTran();
                Context.Db.Updateable(article).ExecuteCommand();
                content.ArticleId = article.Id;
                Context.Db.Updateable(content).WhereColumns(it => it.ArticleId).ExecuteCommand();
                Context.Db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                Context.Db.Ado.RollbackTran();
                _logger.Error(ex.Message);
                return false;
            }
        }
    }
}