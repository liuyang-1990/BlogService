using SqlSugar;

namespace Blog.Model.Db
{
    [SugarTable("tbl_article_comment")]
    public class AtricleComment:BaseEntity
    {
        /// <summary>
        /// 对应文章ID
        /// </summary>
        [SugarColumn(ColumnName = "article_id")]
        public int ArticleId { get; set; }

        [SugarColumn(ColumnName = "comment_id")]
        public int CommentId { get; set; }


    }
}