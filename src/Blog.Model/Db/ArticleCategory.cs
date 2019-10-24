using SqlSugar;

namespace Blog.Model.Db
{
    /// <summary>
    /// 文章内容
    /// </summary>
    [SugarTable("tbl_article_category")]
    public class ArticleCategory:BaseEntity
    {
        /// <summary>
        /// 对应文章ID
        /// </summary>
        [SugarColumn(ColumnName = "article_id")]
        public string ArticleId { get; set; }

        /// <summary>
        /// 对应标签id
        /// </summary>
        [SugarColumn(ColumnName = "category_id")]
        public string CategoryId { get; set; }
    }
}