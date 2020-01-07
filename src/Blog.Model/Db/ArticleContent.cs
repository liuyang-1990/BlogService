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
        [SugarColumn(ColumnName = "article_id", IsNullable = false)]
        public int ArticleId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        //自动转码和解码->方便插入表情，不需要对表做任何配置
        [SugarColumn(IsTranscoding = true, ColumnDataType = "text", IsNullable = false)]
        public string Content { get; set; }

    }
}