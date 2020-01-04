using SqlSugar;

namespace Blog.Model.Db
{
    /// <summary>
    /// 文章标签表
    /// </summary>
    [SugarTable("tbl_article_tag")]
    public class ArticleTag : BaseEntity
    {
        /// <summary>
        /// 对应文章ID
        /// </summary>
        [SugarColumn(ColumnName = "article_id", IsNullable = false)]
        public int ArticleId { get; set; }

        /// <summary>
        /// 对应标签id
        /// </summary>
        [SugarColumn(ColumnName = "tag_id", IsNullable = false)]
        public int TagId { get; set; }


    }
}