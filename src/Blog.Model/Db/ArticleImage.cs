using SqlSugar;

namespace Blog.Model.Db
{
    [SugarTable("tbl_article_image")]
    public class ArticleImage:BaseEntity
    {
        /// <summary>
        /// 对应文章ID
        /// </summary>
        [SugarColumn(ColumnName = "article_id")]
        public int ArticleId { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        [SugarColumn(ColumnName = "image_url")]
        public string ImageUrl { get; set; }
    }
}