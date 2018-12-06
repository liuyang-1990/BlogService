using SqlSugar;

namespace Blog.Model.Db
{
    /// <summary>
    /// 文章内容
    /// </summary>
    [SugarTable("tbl_article_content")]
    public class ArticleContent : BaseEntity
    {
        /// <summary>
        /// 对应文章ID
        /// </summary>
        [SugarColumn(ColumnName = "article_id")]
        public long ArticleId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

    }
}