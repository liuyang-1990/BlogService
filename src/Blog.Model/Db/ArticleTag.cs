using SqlSugar;

namespace Blog.Model.Db
{
    /// <summary>
    /// 文章标签表
    /// </summary>
    [SugarTable("tbl_article_tag")]
    public class ArticleTag:BaseEntity
    {
        /// <summary>
        /// 对应文章ID
        /// </summary>
        [SugarColumn(ColumnName = "article_id")]
        public long ArticleId { get; set; }

        /// <summary>
        /// 对应标签id
        /// </summary>
        [SugarColumn(ColumnName = "tag_id")]
        public int TagId { get; set; }


    }
}