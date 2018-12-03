using System;
using Blog.Infrastructure;
using Blog.Model;
using Microsoft.Extensions.Options;
using NLog;

namespace Blog.Repository.Implement
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ArticleRepository(IOptions<DBSetting> settings) : base(settings)
        {

        }


        public bool Insert(Article article, ArticleContent content)
        {
            try
            {
                Context.Db.Ado.BeginTran();
                var id = Context.Db.Insertable(article).ExecuteReturnIdentity();
                content.Article_Id = id;
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

        public override bool Delete(int id)
        {
            try
            {
                Context.Db.Ado.BeginTran();
                Context.CurrentDb.DeleteById(id);
                Context.Db.Deleteable<ArticleContent>().Where(x => x.Article_Id == id).ExecuteCommand();
                Context.Db.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }


        }

        public bool Update(Article article, ArticleContent content)
        {
            try
            {
                Context.Db.Ado.BeginTran();
                Context.Db.Updateable(article).ExecuteCommand();
                content.Article_Id = article.Id;
                Context.Db.Updateable(content).WhereColumns(it => it.Article_Id).ExecuteCommand();
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