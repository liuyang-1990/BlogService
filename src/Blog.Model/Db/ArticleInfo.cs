using SqlSugar;

namespace Blog.Model.Db
{
    /// <summary>
    /// 文章表
    /// </summary>
    [SugarTable("tbl_article_info")]
    public class ArticleInfo : BaseEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string Abstract { get; set; }

        /// <summary>
        /// 是否原创 1 是  0  不是
        /// </summary>
        [SugarColumn(ColumnName = "is_original")]
        public bool IsOriginal { get; set; }

        /// <summary>
        /// 文章状态 1 发布 0 草稿
        /// </summary>
        [SugarColumn(ColumnName = "is_published")]
        public bool IsPublished { get; set; }

        /// <summary>
        /// 摘要右侧图片
        /// </summary>
        [SugarColumn(ColumnName = "image_url", Length = 200, IsNullable = true)]
        public string ImageUrl { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        [SugarColumn(ColumnName = "is_top")]
        public bool IsTop { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// 评论量
        /// </summary>
        public int Comments { get; set; }

        /// <summary>
        /// 点赞量
        /// </summary>
        public int Likes { get; set; }


    }
}